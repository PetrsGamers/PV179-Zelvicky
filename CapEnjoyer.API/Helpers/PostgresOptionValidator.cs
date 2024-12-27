namespace CapEnjoyer.API.Helpers;

using DAL;

public class PostgresOptionValidator
{
    private readonly string? connectionString;

    public required string? ConnectionString
    {
        get => connectionString;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(ConnectionString), "Connection string cannot be null or empty.");
            }

            connectionString = value;
        }
    }

    public static bool IsValid(WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CapEnjoyerDbContext>();

        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Database connection validated successfully.");
            return true;
        }

        Console.WriteLine("Database is not reachable. Exiting...");
        return false;
    }
}

public static class PostgresOptionValidatorExtensions
{
    public static void ValidateConnection(this WebApplication webApplication)
    {
        if (!PostgresOptionValidator.IsValid(webApplication))
        {
            throw new HttpRequestException("Database validation failed.");
        }
    }
}
