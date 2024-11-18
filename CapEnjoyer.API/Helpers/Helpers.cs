namespace CapEnjoyer.API.Helpers;

using DAL;

public static class Helpers
{
    public static string? GetConnectionString()
    {
        var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                               $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                               $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                               $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                               $"Database={Environment.GetEnvironmentVariable("DB_NAME")}";
        return connectionString;
    }

    public static void CheckIfEnvironmentVariablesAreSet()
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_HOST")) ||
            string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_PORT")) ||
            string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_USERNAME")) ||
            string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_PASSWORD")) ||
            string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_NAME")))
        {
            Console.WriteLine(
                "Please provide all required environment variables in the .env file inside the current Project folder.");
            Environment.Exit(1);
        }
    }

    public static void CheckIfDatabaseServerReachableAndCreated(WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CapEnjoyerDbContext>();
        if (dbContext.Database.CanConnect())
        {
            return;
        }

        Console.WriteLine("Database is not reachable. Exiting...");
        Environment.Exit(1);
        // try
        // {
        //     if (dbContext.Database.CanConnect())
        //     {
        //         Console.WriteLine("Database is reachable. Applying no migrations.");
        //         return;
        //     }
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine("Database is not reachable. Trying to create the db and apply migrations...");
        // }
        //
        // try
        // {
        //     dbContext.Database.Migrate(); TODO: there seems to be a bug in the Migrate method, invetigate later
        //     Console.WriteLine("Database created and migrations applied.");
        // }
        // catch (Exception e)
        // {
        //     // ...if the database is not reachable or there was an error during creation, throw an error
        //     Console.WriteLine(
        //         "The database is not reachable or there was an error during creation. Please check your connection.");
        //     Console.WriteLine(e);
        //     Environment.Exit(1);
        // }
    }
}
