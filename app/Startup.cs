using System.Text.Json.Serialization;
using app.Configs;
using app.Extensions;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace app;

public class Startup
{
    private IConfiguration _config { get; }

    public Startup(IConfiguration configuration)
    {
        configuration.Bind("Auth0", new Auth0Config());
        _config = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<BuildDbContext>();
        services.AddHttpContextAccessor();
        services.Configure<Auth0Settings>(_config.GetSection("Auth0"));

        services.AddCors();
        services.AddAuth();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped<BuildServices>();
        services.AddScoped<UserServices>();
        services.AddScoped<RatingServices>();
        services.AddScoped<CommentServices>();

        services.AddDbContext<BuildDbContext>(options =>
            options.UseSqlServer(_config.GetValue<string>("ConnectionStrings:ApiDatabase")));

        services.AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;

            options.OutputFormatters.RemoveType<StringOutputFormatter>();
        }).AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            o.JsonSerializerOptions.WriteIndented = true;
        });

        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
        services.AddHttpLogging(httpLogging => { httpLogging.LoggingFields = HttpLoggingFields.All; });

        // .AddNewtonsoftJson(options =>
        //     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        // );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpLogging();
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials());

        if (env.IsProduction())
        {
            app.UseSecurityHeaders();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseUserProfile();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}