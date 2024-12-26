namespace Tests;

using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Constants;
using CapEnjoyer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class UserServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        this.context = new CapEnjoyerDbContext(options);
    }

    public void Dispose()
    {
        this.context.Database.EnsureDeleted();
        this.context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async void GetUserByIdReturnsCorrectUser()
    {
        var userService = new UserService(this.context);

        var userId = new Guid("6B3D4C29-85CF-4031-851A-4AB9EAF6E5ED");
        var user = new User
        {
            Id = userId,
            Albums = [],
            Email = "ted@gmail.com",
            Role = Role.User,
            Username = "ted"
        };

        this.context.Users.Add(user);
        await this.context.SaveChangesAsync();

        var result = await userService.GetUserById(userId);
        Assert.Equal(result.Id, userId);
    }
}
