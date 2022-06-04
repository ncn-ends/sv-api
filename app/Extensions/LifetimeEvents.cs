using Microsoft.Data.SqlClient;

namespace app.Extensions;

public static class LifetimeEvents
{
    public static void OnAppStarted(IConfiguration config)
    {
        try
        {
            var connectionString = config.GetValue<string>("ConnectionStrings:ApiDatabase");
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open(); /* An exception will be thrown here if connection is broken */
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nDatabase connection is working\n");
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nDatabase connection failed.\n");
            Console.ResetColor();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}