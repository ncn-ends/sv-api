using System.Net;
using System.Net.Mime;
using System.Security;
using System.Security.Authentication;
using app.Middleware;
using Microsoft.AspNetCore.Diagnostics;

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
    
    public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var responseObject = new Dictionary<string, string>();

                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();

                var exception = exceptionHandlerPathFeature?.Error;

                switch (exception)
                {
                    case AuthenticationException:
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        break;

                    case SecurityException:
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        break;

                    case DataMisalignedException:
                    case ArgumentException:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;

                    case CookieException:
                        context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                        break;

                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        responseObject.Add("msg", "Unknown internal server error.");
                        break;
                }

                await context.Response.WriteAsJsonAsync(responseObject);
            });
        });
    }
}