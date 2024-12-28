namespace Tests;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
using CapEnjoyer.DAL.Constants;
using CapEnjoyer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class AlbumServiceTests : IDisposable
{
    private readonly CapEnjoyerDbContext context;

    public AlbumServiceTests()
    {
        var options = new DbContextOptionsBuilder<CapEnjoyerDbContext>()
            .UseInMemoryDatabase("TestAlbumDatabase")
            .Options;

        context = new CapEnjoyerDbContext(options);
    }

    public void Dispose()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
        GC.SuppressFinalize(this);
    }


    [Fact]
    public async Task GetAlbumByIdReturnsCorrectAlbum()
    {
        var albumService = new AlbumService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);

        var albumId = Guid.NewGuid();
        var album = new Album
        {
            Id = albumId,
            Name = "Album1",
            Description = "Description1",
            Public = true,
            UserId = userId,
            User = user
        };

        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var result = await albumService.GetAlbumById(albumId);

        Assert.NotNull(result);
        Assert.Equal(albumId, result?.Id);
        Assert.Equal("Album1", result?.Name);
        Assert.Equal("Description1", result?.Description);
    }

    [Fact]
    public async Task GetAlbumsReturnsAllAlbums()
    {
        var albumService = new AlbumService(context);
        var userService = new UserService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);


        context.Albums.AddRange(
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Album1",
                Description = "Description1",
                Public = true,
                UserId = userId,
                User = user
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Album2",
                Description = "Description2",
                Public = true,
                UserId = userId,
                User = user
            }
        );
        await context.SaveChangesAsync();

        var result = await albumService.GetAllAlbums();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateAlbumAddsNewAlbum()
    {
        var albumService = new AlbumService(context);
        var userService = new UserService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);

        var albumDto =
            new AlbumInsertDto { Name = "Album1", Description = "Description1", Public = true, User = userId };
        var createdAlbum = await albumService.CreateAlbum(albumDto);
        await context.SaveChangesAsync();

        var result = await context.Albums.FindAsync(createdAlbum.Id);

        Assert.NotNull(result);
        Assert.Equal(createdAlbum.Id, result?.Id);
        Assert.Equal("Album1", result?.Name);
        Assert.Equal("Description1", result?.Description);
    }

    [Fact]
    public async Task CreateAlbumThrowsExceptionWhenNameOrDescriptionIsMissing()
    {
        var albumService = new AlbumService(context);

        var albumDto = new AlbumInsertDto { Name = "", Description = "Description1" };

        await Assert.ThrowsAsync<ArgumentException>(() => albumService.CreateAlbum(albumDto));
    }

    [Fact]
    public async Task DeleteAlbumRemovesAlbum()
    {
        var albumService = new AlbumService(context);
        var userService = new UserService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);

        var albumId = Guid.NewGuid();
        var album = new Album
        {
            Id = albumId,
            Name = "Album1",
            Description = "Description1",
            Public = true,
            UserId = userId,
            User = user
        };

        context.Albums.Add(album);
        await context.SaveChangesAsync();

        await albumService.DeleteAlbum(albumId);

        var deletedAlbum = await context.Albums.FindAsync(albumId);
        Assert.Null(deletedAlbum);
    }

    [Fact]
    public async Task UpdateAlbumUpdatesAlbum()
    {
        var albumService = new AlbumService(context);
        var userService = new UserService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);

        var albumId = Guid.NewGuid();
        var album = new Album
        {
            Id = albumId,
            Name = "Album1",
            Description = "Description1",
            Public = true,
            UserId = userId,
            User = user
        };

        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var updatedAlbum = new AlbumInsertDto { Name = "Album2", Description = "Description2" };
        await albumService.UpdateAlbum(albumId, updatedAlbum);

        var result = await context.Albums.FindAsync(albumId);

        Assert.NotNull(result);
        Assert.Equal(albumId, result?.Id);
        Assert.Equal("Album2", result?.Name);
        Assert.Equal("Description2", result?.Description);
    }

    [Fact]
    public async Task UpdateAlbumThrowsExceptionWhenAlbumNotFound()
    {
        var albumService = new AlbumService(context);

        var albumId = Guid.NewGuid();
        var updatedAlbum = new AlbumInsertDto { Name = "Album2", Description = "Description2" };

        await Assert.ThrowsAsync<ArgumentException>(() => albumService.UpdateAlbum(albumId, updatedAlbum));
    }


    [Fact]
    public async Task AddCapToAlbumAddsCapToAlbum()
    {
        var albumService = new AlbumService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);

        var albumId = Guid.NewGuid();
        var album = new Album
        {
            Id = albumId,
            Name = "Album1",
            Description = "Description1",
            Public = true,
            UserId = userId,
            User = user
        };

        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var capId = Guid.NewGuid();
        var cap = new Cap { Id = capId, TextOnCap = "Cap1", Description = "Description1", CapPicture = "Picture1" };
        context.Caps.Add(cap);

        await albumService.AddCapToAlbum(albumId, capId);

        var result = await context.CapToAlbums.FirstOrDefaultAsync(cl => cl.AlbumId == albumId && cl.CapId == capId);

        Assert.NotNull(result);
        Assert.Equal(albumId, result?.AlbumId);
        Assert.Equal(capId, result?.CapId);
    }

    [Fact]
    public async Task AddCapToAlbumThrowsExceptionWhenAlbumNotFound()
    {
        var albumService = new AlbumService(context);

        var albumId = Guid.NewGuid();
        var capId = Guid.NewGuid();
        var cap = new Cap { Id = capId, TextOnCap = "Cap1", Description = "Description1", CapPicture = "Picture1" };
        context.Caps.Add(cap);

        await Assert.ThrowsAsync<ArgumentException>(() => albumService.AddCapToAlbum(albumId, capId));
    }

    [Fact]
    public async Task AddCapToAlbumThrowsExceptionWhenCapNotFound()
    {
        var albumService = new AlbumService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);

        var albumId = Guid.NewGuid();
        var album = new Album
        {
            Id = albumId,
            Name = "Album1",
            Description = "Description1",
            Public = true,
            UserId = userId,
            User = user
        };

        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var capId = Guid.NewGuid();

        await Assert.ThrowsAsync<ArgumentException>(() => albumService.AddCapToAlbum(albumId, capId));
    }

    [Fact]
    public async Task RemoveCapFromAlbumRemovesCapFromAlbum()
    {
        var albumService = new AlbumService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);

        var albumId = Guid.NewGuid();
        var album = new Album
        {
            Id = albumId,
            Name = "Album1",
            Description = "Description1",
            Public = true,
            UserId = userId,
            User = user
        };

        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var capId = Guid.NewGuid();
        var cap = new Cap { Id = capId, TextOnCap = "Cap1", Description = "Description1", CapPicture = "Picture1" };
        context.Caps.Add(cap);

        await albumService.AddCapToAlbum(albumId, capId);
        await albumService.RemoveCapFromAlbum(albumId, capId);

        var result = await context.CapToAlbums.FirstOrDefaultAsync(cl => cl.AlbumId == albumId && cl.CapId == capId);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveCapFromAlbumThrowsExceptionWhenCapNotFoundInAlbum()
    {
        var albumService = new AlbumService(context);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "email",
            Username = "User1",
            Albums = [],
            Role = Role.User
        };
        context.Users.Add(user);

        var albumId = Guid.NewGuid();
        var album = new Album
        {
            Id = albumId,
            Name = "Album1",
            Description = "Description1",
            Public = true,
            UserId = userId,
            User = user
        };

        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var capId = Guid.NewGuid();
        var cap = new Cap { Id = capId, TextOnCap = "Cap1", Description = "Description1", CapPicture = "Picture1" };
        context.Caps.Add(cap);

        await Assert.ThrowsAsync<ArgumentException>(() => albumService.RemoveCapFromAlbum(albumId, capId));
    }

    [Fact]
    public async Task RemoveCapFromAlbumThrowsExceptionWhenAlbumNotFound()
    {
        var albumService = new AlbumService(context);

        var albumId = Guid.NewGuid();
        var capId = Guid.NewGuid();
        var cap = new Cap { Id = capId, TextOnCap = "Cap1", Description = "Description1", CapPicture = "Picture1" };
        context.Caps.Add(cap);

        await Assert.ThrowsAsync<ArgumentException>(() => albumService.RemoveCapFromAlbum(albumId, capId));
    }
}
