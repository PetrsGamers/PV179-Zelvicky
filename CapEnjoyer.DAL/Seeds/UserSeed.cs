namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Constants;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class UserSeed
{
    private const string UsersSeedString = "basic_user_seed";
    private const string Domain = "@fitmuni.cz";
    private static readonly List<string> DefaultUsers = ["admin", "bivaD", "goretexak", "Ted", "Monke", "Senator"];

    public static List<User> Seed(ModelBuilder modelBuilder)
    {
        Randomizer.Seed = SeedUtils.GetRandom(UsersSeedString);

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid())
            .RuleFor(u => u.Username, (f) => f.Name.FirstName())
            .RuleFor(u => u.Email, (f) => f.Internet.Email())
            .RuleFor(u => u.Password, "password")
            .RuleFor(u => u.Role, (f, u) => u.Username == "admin" ? Role.Admin : Role.User);

        List<User> users = [];
        foreach (var userName in DefaultUsers)
        {
            var user = userFaker.Generate();
            user.Username = userName;
            user.Email = userName.ToLowerInvariant() + Domain;
            users.Add(user);
        }

        modelBuilder.Entity<User>().HasData(users);
        return users;
    }
}
