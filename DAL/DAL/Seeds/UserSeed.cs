namespace DAL.Seeds;

using Constants;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class UserSeed
{
    public static List<User> Seed(ModelBuilder modelBuilder)

    {
        var users = new List<User>([
            new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@fitmuni.cz",
                Password = "adminpassword",
                Role = Role.Admin
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "bivaD",
                Email = "bivad@fitmuni.cz",
                Password = "password1",
                Role = Role.User
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "goretexák",
                Email = "goretexak@trombon.no",
                Password = "password2",
                Role = Role.User
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "koviďák",
                Email = "kovidak@fitmuni.cz",
                Password = "password3",
                Role = Role.User
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "ucitelkaLover69",
                Email = "lover69@fitmuni.cz",
                Password = "password4",
                Role = Role.User
            },
            new User
            {
                Id = Guid.NewGuid(),
                Username = "Igorko",
                Email = "igorko@fitmuni.cz",
                Password = "password5",
                Role = Role.User
            }
        ]);
        modelBuilder.Entity<User>().HasData(users);

        return users;
    }
}
