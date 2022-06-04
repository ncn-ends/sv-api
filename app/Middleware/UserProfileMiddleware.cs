using app.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace app.Middleware;

public class UserProfileMiddleware
{
    private readonly RequestDelegate _next;

    public UserProfileMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // TODO: similar test for this has already potentially been created in UtilityTests.TestUserBroker_Tests
    /*  Populates "UserProfile" item of HttpContext with an instance of Auth0User
     */
    public async Task Invoke(HttpContext context)
    {
        var userInfoClaim = context.User.Claims.FirstOrDefault(c => c.Type == "https://sv.com/info");

        if (userInfoClaim == null)
        {
            await _next(context);
            return;
        }

        var deserializedUserInfo = JsonConvert.DeserializeObject<TokenUser>(userInfoClaim.Value);

        if (deserializedUserInfo == null)
        {
            await _next(context);
            return;
        }

        context.Items["UserProfile"] = deserializedUserInfo;
        await _next(context);
    }
}