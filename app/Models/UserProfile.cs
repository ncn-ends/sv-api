using System.ComponentModel.DataAnnotations;
using app.Models;
using Microsoft.EntityFrameworkCore;

namespace app.Models;

// [Index(nameof(UserProfileId), IsUnique = true)]
public class UserProfile : BaseEntity
{
    [Key]
    public string UserProfileId { get; set; }

    public string GameProfileLink { get; set; } = "";
    public string TwitchLink { get; set; } = "";
    public string GamerTag { get; set; } = "";

    [DiscordId]
    public string DiscordId { get; set; } = "";

    [MaxLength(200)]
    public string ProfileBio { get; set; } = "";

    public List<Rating> Ratings { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<BuildOrderList> BuildOrderLists { get; set; } = new();

    public int LoginCount { get; set; } = Int32.MinValue;
    public string CountryCode { get; set; } = "";
    public string TimeZone { get; set; } = "";
    public bool EmailVerified { get; set; }
    public string Email { get; set; } = "";
    public string UserId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Picture { get; set; } = "";
}

public class UserProfileDto : BaseEntity
{
    public string? UserProfileId { get; set; }
    public string GameProfileLink { get; set; } = "";
    public string TwitchLink { get; set; } = "";
    public string GamerTag { get; set; } = "";
    public string DiscordId { get; set; } = "";
    public string ProfileBio { get; set; } = "";
    public int LoginCount { get; set; }
    public string CountryCode { get; set; } = "";
    public string TimeZone { get; set; } = "";
    public bool EmailVerified { get; set; }
    public string Email { get; set; } = "";
    public string UserId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Picture { get; set; } = "";
    public List<RatingDto>? Ratings { get; set; }
    public List<CommentDto>? Comments { get; set; }
    public List<BuildOrderListDto>? BuildOrderLists { get; set; }
}

// "ip": event.request.ip, // string
// "city": event.request.geoip.cityName, // string
// "country": event.request.geoip.countryName, // string
// "countryCode": event.request.geoip.countryCode3, // string
// "latitude": event.request.geoip.latitude, // float
// "longitude": event.request.geoip.longitude, // float
// "timeZone": event.request.geoip.timeZone, // string
// "loginCount": event.stats.logins_count, // integer
// "emailVerified": event.user.email_verified, // bool
// "email": event.user.email, // string
// "userId": event.user.user_id, // string
// "name": event.user.name, // string
// "nickname": event.user.nickname, // string
// "picture": event.user.picture // string(url)
public class Auth0UserDataDto
{
    public string Secret { get; set; } = "";
    public int LoginCount { get; set; }
    public string CountryCode { get; set; } = "";
    public string TimeZone { get; set; } = "";
    public bool EmailVerified { get; set; }
    public string Email { get; set; } = "";
    public string UserId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Nickname { get; set; } = "";
    public string Picture { get; set; } = "";
}