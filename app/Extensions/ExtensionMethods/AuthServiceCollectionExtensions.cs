using System.Security.Claims;
using app.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace app.Extensions;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(
        this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = Auth0Config.Current.Authority;
                options.Audience = Auth0Config.Current.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("read:messages",
                policy => policy.Requirements.Add(new HasScopeRequirement("read:messages",
                    Auth0Config.Current.Authority)));
        });


        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        return services;
    }
}