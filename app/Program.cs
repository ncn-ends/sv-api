namespace app;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(AddAppConfiguration)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    public static void AddAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
    {
        config.Sources.Clear();
        var env = hostingContext.HostingEnvironment;
        config.AddJsonFile("appsettings.json");
        config.AddJsonFile("appsettings.Development.json");

        if (env.IsProduction()) config.AddEnvironmentVariables();
        else config.AddUserSecrets<Startup>();
    }
}