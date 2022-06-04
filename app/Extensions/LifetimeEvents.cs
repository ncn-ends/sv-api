using Microsoft.Data.SqlClient;

namespace app.Extensions;

public static class LifetimeEvents
{
    public static void OnAppStarted(string connectionString)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open(); /* An exception will be thrown here if connection is broken */
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nDatabase connection is working\n");
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            
            var outputMsg =
                "\n(LifetimeEvents.cs - line: 20) Database connection failed. Check to make sure connection string is valid.";
            Console.WriteLine(outputMsg);
            
            Console.ResetColor();
            
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}