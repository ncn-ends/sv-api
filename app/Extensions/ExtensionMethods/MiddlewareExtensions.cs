using app.Middleware;

namespace app.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseUserProfile(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UserProfileMiddleware>();
    }

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UniversalHeadersMiddleware>();
    }
}