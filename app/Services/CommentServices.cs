using System.Net;
using System.Security;
using System.Security.Authentication;
using app.Configs;
using app.Controllers;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app.Services;

public class CommentServices
{
    private readonly BuildDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentServices(
        BuildDbContext dbContext, 
        IHttpContextAccessor httpContextAccessorAccessor)
    {
        _httpContextAccessor = httpContextAccessorAccessor;
        _dbContext = dbContext;
    }

    public async Task<CommentEndpointBody> SubmitComment(CommentEndpointBody commentBody)
    {
        var authUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (authUser == null) throw new AuthenticationException("User profile was not set from auth token.");
        var userRecord = _dbContext.UserProfiles.FirstOrDefault(u => u.UserProfileId == authUser.profile_id);
        if (userRecord == null) throw new CookieException();

        var listRecord = _dbContext.BuildOrderLists
            .Include(b => b.Comments)
            .FirstOrDefault(b => b.BuildOrderListId == commentBody.listId);

        if (listRecord is null) throw new ArgumentException();

        var freshComment = new Comment
        {
            Body = commentBody.body
        };
        listRecord.Comments.Add(freshComment);
        userRecord.Comments.Add(freshComment);
        await _dbContext.SaveChangesAsync();
        return commentBody;
    }

    public async Task<CommentEndpointBody> UpdateComment(CommentEndpointBody commentBody)
    {
        var authUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (authUser == null) throw new AuthenticationException("User profile was not set from auth token.");
    
        var userRecord = _dbContext.UserProfiles.FirstOrDefault(u => u.UserProfileId == authUser.profile_id);
        if (userRecord == null) throw new CookieException();

        var storedComment = _dbContext.Comments
            .FirstOrDefault(r => r.CommentId == commentBody.commentId);
        if (storedComment == null) throw new ArgumentException("Comment not found.");
        if (storedComment.UserProfileId != userRecord.UserProfileId)
        {
            throw new SecurityException("Attempting to update comment that does not belong to user.");
        }
        
        if (storedComment.Body != commentBody.body) storedComment.Body = commentBody.body;
    
        await _dbContext.SaveChangesAsync();
        return commentBody;
    }
    
    public async Task<int> DeleteComment(int commentId)
    {
        var authUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (authUser == null) throw new AuthenticationException("User profile was not set from auth token.");
    
        var userRecord = _dbContext.UserProfiles.FirstOrDefault(u => u.UserProfileId == authUser.profile_id);
        if (userRecord == null) throw new CookieException();
    
        var commentToRemove = _dbContext.Comments
            .FirstOrDefault(c => c.CommentId == commentId);
        if (commentToRemove == null) throw new ArgumentException($"Comment not found. Comment ID: {commentId}");
    
        _dbContext.Remove(commentToRemove);
    
        await _dbContext.SaveChangesAsync();
        return commentId;
    }
}