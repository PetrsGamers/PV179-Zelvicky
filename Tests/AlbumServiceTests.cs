namespace Tests;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services;
using CapEnjoyer.DAL;
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

        this.context = new CapEnjoyerDbContext(options);
    }

    public void Dispose()
    {
        this.context.Database.EnsureDeleted();
        this.context.Dispose();
    }


    [Fact]
    public async Task GetAlbumByIdReturnsCorrectAlbum()
    {
        var albumService = new AlbumService(this.context);

        var albumId = Guid.NewGuid();
        var album = new Album { Id = albumId, Name = "Album1", Description = "Description1" };

        this.context.Albums.Add(album);
        await this.context.SaveChangesAsync();

        var result = await albumService.GetAlbumById(albumId);

        Assert.NotNull(result);
        Assert.Equal(albumId, result?.Id);
        Assert.Equal("Album1", result?.Name);
        Assert.Equal("Description1", result?.Description);
    }

    [Fact]
    public async Task GetAlbumsReturnsAllAlbums()
    {
        var albumService = new AlbumService(this.context);

        this.context.Albums.AddRange(
            new Album { Id = Guid.NewGuid(), Name = "Album1", Description = "Description1" },
            new Album { Id = Guid.NewGuid(), Name = "Album2", Description = "Description2" }
        );
        await this.context.SaveChangesAsync();

        var result = await albumService.GetAllAlbums();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateAlbumAddsNewAlbum()
    {
        var albumService = new AlbumService(this.context);

        var albumId = Guid.NewGuid();
        var album = new Album { Id = albumId, Name = "Album1", Description = "Description1" };
        var albumDto = new AlbumInsertDto { Name = album.Name, Description = album.Description };
        await albumService.CreateAlbum(albumDto);

        var result = await this.context.Albums.FindAsync(albumId);

        Assert.NotNull(result);
        Assert.Equal(albumId, result?.Id);
        Assert.Equal("Album1", result?.Name);
        Assert.Equal("Description1", result?.Description);
    }


}
