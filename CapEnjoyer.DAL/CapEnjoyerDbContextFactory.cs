using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CapEnjoyer.DAL
{
    using DotNetEnv;

    public class CapEnjoyerDbContextFactory : IDesignTimeDbContextFactory<CapEnjoyerDbContext>
    {
        public CapEnjoyerDbContext CreateDbContext(string[] args)
        {
            Env.Load();
            var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                                   $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                                   $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                                   $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                                   $"Database={Environment.GetEnvironmentVariable("DB_NAME")}";

            var optionsBuilder = new DbContextOptionsBuilder<CapEnjoyerDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new CapEnjoyerDbContext(optionsBuilder.Options);
        }
    }
}
