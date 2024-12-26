namespace Tests;

using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Constants;
using CapEnjoyer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class LeaderboardServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public LeaderboardServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestLeaderboardDatabase")
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
    public async Task GetLeaderboardReturnsCorrectRankings()
    {
        // Arrange
        var leaderboardService = new LeaderboardService(this.context);

        var user1 = new User { Id = Guid.NewGuid(), Email = "e1@example.com", Username = "User1", Albums = [], Role = Role.User };
        var user2 = new User { Id = Guid.NewGuid(), Email = "e2@example.com", Username = "User2", Albums = [], Role = Role.User };
        var cap1 = new Cap { Id = Guid.NewGuid(), TextOnCap = "Cap1", CapPicture = "Cap1picture", Description = "Cap1description" };
        var cap2 = new Cap { Id = Guid.NewGuid(), TextOnCap = "Cap2", CapPicture = "Cap2picture", Description = "Cap2description" };
        var cap3 = new Cap { Id = Guid.NewGuid(), TextOnCap = "Cap3", CapPicture = "Cap3picture", Description = "Cap3description" };

        var album1id = Guid.NewGuid();
        user1.Albums.Add(new Album
        {
            Id = album1id,
            CapLinks =
            [
                new CapToAlbum { Cap = cap1, CapId = cap1.Id, AlbumId = album1id },
                new CapToAlbum { Cap = cap2, CapId = cap2.Id, AlbumId = album1id }
            ],
            Description = "Description1",
            Name = "Name1",
            Public = true,
            User = user1,
            UserId = user1.Id

        });
        var album2id = Guid.NewGuid();
        user1.Albums.Add(new Album
        {
            Id = album2id,
            CapLinks =
            [
                new CapToAlbum { Cap = cap2, CapId = cap2.Id, AlbumId = album2id },
                new CapToAlbum { Cap = cap3, CapId = cap3.Id, AlbumId = album2id }
            ],
            Description = "Description1",
            Name = "Name1",
            Public = true,
            User = user1,
            UserId = user1.Id
        });

        var album3id = Guid.NewGuid();
        user2.Albums.Add(new Album
        {
            Id = Guid.NewGuid(),
            CapLinks = [new CapToAlbum { Cap = cap1, CapId = cap1.Id, AlbumId = album3id }],
            Description = "Description2",
            Name = "Name2",
            Public = true,
            User = user2,
            UserId = user2.Id
        });

        this.context.Users.AddRange(user1, user2);
        this.context.Caps.AddRange(cap1, cap2, cap3);
        await this.context.SaveChangesAsync();

        // Act
        var leaderboard = await leaderboardService.GetLeaderboard();

        // Assert
        Assert.NotNull(leaderboard);
        Assert.Equal(2, leaderboard.Count);

        var user1Result = leaderboard.First(l => l.Username == "User1");
        var user2Result = leaderboard.First(l => l.Username == "User2");

        Assert.Equal(1, user1Result.Rank);
        Assert.Equal(3, user1Result.DistinctCapCount);

        Assert.Equal(2, user2Result.Rank);
        Assert.Equal(1, user2Result.DistinctCapCount);
    }

    [Fact]
    public async Task GetLeaderboardHandlesUsersWithNoAlbums()
    {
        // Arrange
        var leaderboardService = new LeaderboardService(this.context);

        var user1 = new User { Id = Guid.NewGuid(), Email = "e1@example.com", Username = "User1", Albums = [], Role = Role.User };
        var user2 = new User { Id = Guid.NewGuid(), Email = "e2@example.com", Username = "User2", Albums = [], Role = Role.User };

        this.context.Users.AddRange(user1, user2);
        await this.context.SaveChangesAsync();

        // Act
        var leaderboard = await leaderboardService.GetLeaderboard();

        // Assert
        Assert.NotNull(leaderboard);
        Assert.Equal(2, leaderboard.Count);

        foreach (var entry in leaderboard)
        {
            Assert.Equal(0, entry.DistinctCapCount);
        }
    }

    [Fact]
    public async Task GetLeaderboardHandlesTieInDistinctCapCount()
    {
        // Arrange
        var leaderboardService = new LeaderboardService(this.context);

        var user1 = new User { Id = Guid.NewGuid(), Email = "e1@example.com", Username = "User1", Albums = [], Role = Role.User };
        var user2 = new User { Id = Guid.NewGuid(), Email = "e2@example.com", Username = "User2", Albums = [], Role = Role.User };
        var cap1 = new Cap { Id = Guid.NewGuid(), TextOnCap = "Cap1", CapPicture = "Cap1picture", Description = "Cap1description" };
        var cap2 = new Cap { Id = Guid.NewGuid(), TextOnCap = "Cap2", CapPicture = "Cap2picture", Description = "Cap2description" };

        var album1id = Guid.NewGuid();
        user1.Albums.Add(new Album
        {
            Id = album1id,
            CapLinks =
            [
                new CapToAlbum { Cap = cap1, CapId = cap1.Id, AlbumId = album1id },
                new CapToAlbum { Cap = cap2, CapId = cap2.Id, AlbumId = album1id }
            ],
            Description = "Description1",
            Name = "Name1",
            Public = true,
            User = user1,
            UserId = user1.Id
        });


        var album2id = Guid.NewGuid();
        user2.Albums.Add(new Album
        {
            Id = Guid.NewGuid(),
            CapLinks =
            [
                new CapToAlbum { Cap = cap1, CapId = cap1.Id, AlbumId = album2id },
                new CapToAlbum { Cap = cap2, CapId = cap2.Id, AlbumId = album2id }
            ],
            Description = "Description2",
            Name = "Name2",
            Public = true,
            User = user2,
            UserId = user2.Id
        });

        this.context.Users.AddRange(user1, user2);
        this.context.Caps.AddRange(cap1, cap2);
        await this.context.SaveChangesAsync();

        // Act
        var leaderboard = await leaderboardService.GetLeaderboard();

        // Assert
        Assert.NotNull(leaderboard);
        Assert.Equal(2, leaderboard.Count);

        Assert.All(leaderboard, entry => Assert.Equal(2, entry.DistinctCapCount));
        Assert.Equal(1, leaderboard.First().Rank);
        Assert.Equal(2, leaderboard.Last().Rank);
    }
}
