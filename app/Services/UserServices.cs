using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Security.Authentication;
using app.Configs;
using app.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace app.Services;

public class UserServices
{
    private readonly BuildDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Auth0Settings _config;

    public UserServices(BuildDbContext dbContext, IHttpContextAccessor httpContextAccessorAccessor,
        IOptions<Auth0Settings> config)
    {
        _httpContextAccessor = httpContextAccessorAccessor;
        _dbContext = dbContext;
        _config = config.Value;
    }

    public UserProfileDto GetUserProfileById(string profileId)
    {
        var foundUser = _dbContext.UserProfiles
            .Include(u => u.BuildOrderLists)
            .Include(u => u.Comments)
            .Include(u => u.Ratings)
            .FirstOrDefault(u => u.UserProfileId == profileId);
        if (foundUser == null) throw new ArgumentException();

        return foundUser.Adapt<UserProfileDto>();
    }

    public UserProfileDto GetMyUserProfile()
    {
        var authUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (authUser == null) throw new AuthenticationException("User profile was not set from auth token.");
        var userRecord = _dbContext.UserProfiles
            .Include(u => u.Comments)
            .Include(u => u.Ratings)
            .Include(u => u.BuildOrderLists)
            .FirstOrDefault(u => u.UserProfileId == authUser.profile_id);

        if (userRecord == null)
            throw new DataMisalignedException(
                "User had a token, but the token credentials did not match a user in the database.");

        var userRecordAsDto = userRecord.Adapt<UserProfileDto>();
        return userRecordAsDto;
    }

    public void DeleteUsersProfile()
    {
        var authUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (authUser == null) throw new AuthenticationException("User profile was not set from auth token.");
        var userRecord = _dbContext.UserProfiles
            .Include(u => u.Comments)
            .Include(u => u.Ratings)
            .Include(u => u.BuildOrderLists)
            .FirstOrDefault(u => u.UserProfileId == authUser.profile_id);

        if (userRecord == null) return;

        _dbContext.UserProfiles.Remove(userRecord);
        _dbContext.SaveChanges();
    }

    /*  Syncs user profile entity with auth0 user
     *      Used primarily when users update profile, but is also used when users sign up initially
     */
    public async Task<UserProfileDto> SyncUserProfile(UserProfileDto userProfileDto)
    {
        var tokenUser = _httpContextAccessor.HttpContext.Items["UserProfile"] as TokenUser;
        if (tokenUser == null) throw new AuthenticationException("User profile was not set from auth token.");

        userProfileDto.UserProfileId = tokenUser.profile_id;
        var record = _dbContext.UserProfiles.FirstOrDefault(p => p.UserProfileId == tokenUser.profile_id);

        /*
         * Create user record if record doesn't exist
         * TODO: can possibly delete, as user record could potentially never be null with Auth0 changes. Check again later
         */
        if (record == null)
        {
            var freshUser = userProfileDto.Adapt<UserProfile>();
            _dbContext.UserProfiles.Add(freshUser);
            await _dbContext.SaveChangesAsync();

            return userProfileDto;
        }

        record.DiscordId = userProfileDto.DiscordId;
        record.Nickname = userProfileDto.Nickname;
        record.TwitchLink = userProfileDto.TwitchLink;
        record.GamerTag = userProfileDto.GamerTag;
        record.ProfileBio = userProfileDto.ProfileBio;
        record.GameProfileLink = userProfileDto.GameProfileLink;

        await _dbContext.SaveChangesAsync();

        
        return record.Adapt<UserProfileDto>();
    }

    public async Task<Auth0UserDataDto> SyncUserAuth(Auth0UserDataDto auth0UserDataDto)
    {
        if (_config.ApiSecret != auth0UserDataDto.Secret) throw new AuthenticationException("Secret mismatch.");

        var userRecord =
            _dbContext.UserProfiles.FirstOrDefault(u =>
                u.UserProfileId == TokenUser.ParseUserId(auth0UserDataDto.UserId));

        if (userRecord == null)
        {
            var mappedUser = auth0UserDataDto.Adapt<UserProfile>();
            mappedUser.UserProfileId = TokenUser.ParseUserId(auth0UserDataDto.UserId);
            _dbContext.UserProfiles.Add(mappedUser);
            await _dbContext.SaveChangesAsync();
            return auth0UserDataDto;
        }

        /* Not looping through properties; uses reflection */
        var record = userRecord;
        var dto = auth0UserDataDto;
        if (record.CountryCode != dto.CountryCode) record.CountryCode = dto.CountryCode;
        if (record.Email != dto.Email) record.Email = dto.Email;
        if (record.EmailVerified != dto.EmailVerified) record.EmailVerified = dto.EmailVerified;
        if (record.LoginCount != dto.LoginCount) record.LoginCount = dto.LoginCount;
        if (record.Name != dto.Name) record.Name = dto.Name;
        if (record.Nickname != dto.Nickname) record.Nickname = dto.Nickname;
        if (record.Picture != dto.Picture) record.Picture = dto.Picture;
        if (record.TimeZone != dto.TimeZone) record.TimeZone = dto.TimeZone;
        await _dbContext.SaveChangesAsync();

        return auth0UserDataDto;
    }

    public async Task<int> RegisterUser(RegisteringUserDto user)
    {
        var freshUser = new UserProfile
        {
            UserProfileId = TokenUser.ParseUserId(user.user_id)
        };

        _dbContext.UserProfiles.Add(freshUser);
        await _dbContext.SaveChangesAsync();
        return 1;
    }
}