namespace CapEnjoyer.API.Helpers;

using DAL;

public class PostgresOptionValidator
{
    public required string? Host { get; set; }
    public required string? Port { get; set; }
    public required string? Username { get; set; }
    public required string? Password { get; set; }
    public required string? Database { get; set; }

    public string ConnectionString =>
        $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database}";

    public bool IsValid(WebApplication webApplication)
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
    public static void ValidateConnection(this WebApplication webApplication, PostgresOptionValidator validator)
    {
        if (!validator.IsValid(webApplication))
        {
            throw new HttpRequestException("Database validation failed.");
        }
    }
}

// Then in program.cs all you need to do is:
// var mssqlValidator = new MssqlOptionValidator { ... set the data here ... }
// app.ValidateConnection(mssqlValidator);
