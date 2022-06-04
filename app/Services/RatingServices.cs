using System.Net;
using System.Security.Authentication;
using app.Configs;
using app.Controllers;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app.Services;

public class RatingServices
{
    private readonly BuildDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RatingServices(BuildDbContext dbContext, IHttpContextAccessor httpContextAccessorAccessor)
    {
        _httpContextAccessor = httpContextAccessorAccessor;
        _dbContext = dbContext;
    }

    public async Task<RatingEndpointBody> SubmitRating(RatingEndpointBody ratingBody)
    {
        var authUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (authUser == null) throw new AuthenticationException("User profile was not set from auth token.");
        var userRecord = _dbContext.UserProfiles.FirstOrDefault(u => u.UserProfileId == authUser.profile_id);
        if (userRecord == null) throw new CookieException();

        var listRecord = _dbContext.BuildOrderLists
            .Include(b => b.Ratings)
            .FirstOrDefault(b => b.BuildOrderListId == ratingBody.listId);

        if (listRecord is null)
        {
            throw new ArgumentException();
        }

        var freshRating = new Rating
        {
            Value = ratingBody.rating
        };
        listRecord.Ratings.Add(freshRating);
        userRecord.Ratings.Add(freshRating);
        await _dbContext.SaveChangesAsync();
        return ratingBody;
    }

    public async Task<RatingEndpointBody> UpdateRating(RatingEndpointBody ratingBody)
    {
        var authUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (authUser == null) throw new AuthenticationException("User profile was not set from auth token.");

        var userRecord = _dbContext.UserProfiles.FirstOrDefault(u => u.UserProfileId == authUser.profile_id);
        if (userRecord == null) throw new CookieException();

        var listRecord = _dbContext.BuildOrderLists
            .Include(b => b.Ratings)
            .FirstOrDefault(b => b.BuildOrderListId == ratingBody.listId);
        if (listRecord == null) throw new ArgumentException("Build Order List not found.");

        var ratingToChange = listRecord.Ratings
            .FirstOrDefault(r => r.UserProfileId == userRecord.UserProfileId);
        if (ratingToChange == null) throw new ArgumentException("Rating not found.");

        if (ratingToChange.Value != ratingBody.rating) ratingToChange.Value = ratingBody.rating;

        await _dbContext.SaveChangesAsync();
        return ratingBody;
    }

    public async Task<int> DeleteRating(int listId)
    {
        var authUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (authUser == null) throw new AuthenticationException("User profile was not set from auth token.");

        var userRecord = _dbContext.UserProfiles.FirstOrDefault(u => u.UserProfileId == authUser.profile_id);
        if (userRecord == null) throw new CookieException();

        var listRecord = _dbContext.BuildOrderLists
            .Include(b => b.Ratings)
            .FirstOrDefault(b => b.BuildOrderListId == listId);
        if (listRecord == null) throw new ArgumentException("Build Order List not found.");

        var ratingToRemove = listRecord.Ratings
            .FirstOrDefault(r => r.UserProfileId == userRecord.UserProfileId);
        if (ratingToRemove == null) throw new ArgumentException("Rating not found.");

        _dbContext.Remove(ratingToRemove);

        await _dbContext.SaveChangesAsync();
        return listId;
    }
}