namespace CapEnjoyer.BL.Services;

using BL.Interfaces;
using DAL;
using DAL.Entities;
using DTOs;
using Microsoft.EntityFrameworkCore;




public class AlbumService(CapEnjoyerDbContext dbContext) : IAlbumService
{
    private readonly CapEnjoyerDbContext context = dbContext;

    public async Task<IEnumerable<AlbumDto>> GetAllAlbums()
    {
        var albums = await this.context.Albums
            .Select(a => new AlbumDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Public = a.Public,
                User = a.UserId,
                Caps = a.CapLinks.Select(cl => cl.CapId).ToList()
            })
            .ToListAsync();

        return albums;
    }

    public async Task<AlbumDto> GetAlbumById(Guid id)
    {
        var album = await this.context.Albums
            .Where(a => a.Id == id)
            .Select(a => new AlbumDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Public = a.Public,
                User = a.UserId,
                Caps = a.CapLinks.Select(cl => cl.CapId).ToList()
            })
            .FirstOrDefaultAsync() ?? throw new ArgumentException($"Album with ID {id} not found.");

        return album;
    }

    public async Task<AlbumDto> CreateAlbum(AlbumInsertDto album)
    {
        if (string.IsNullOrEmpty(album.Name) || string.IsNullOrEmpty(album.Description))
        {
            throw new ArgumentException("Name and Description are required.");
        }

        var newId = Guid.NewGuid();
        var newAlbum = new Album
        {
            Id = newId,
            Name = album.Name,
            Description = album.Description,
            Public = album.Public,
            UserId = album.User
        };
        var capLinks = new List<CapToAlbum>();
        if (album.Caps != null)
        {
            capLinks = album.Caps.Select(capId => new CapToAlbum { AlbumId = newId, CapId = capId }).ToList();

        }

        await this.context.Albums.AddAsync(newAlbum);
        await this.context.CapToAlbums.AddRangeAsync(capLinks);
        await this.context.SaveChangesAsync();

        return new AlbumDto
        {
            Id = newAlbum.Id,
            Name = newAlbum.Name,
            Description = newAlbum.Description,
            Public = newAlbum.Public,
            User = newAlbum.UserId,
        };
    }

    public async Task<AlbumDto> UpdateAlbum(Guid id, AlbumInsertDto album)
    {
        var existingAlbum = await this.context.Albums.FindAsync(id) ?? throw new ArgumentException("Album not found.");

        existingAlbum.Name = album.Name;
        existingAlbum.Description = album.Description;
        existingAlbum.Public = album.Public;
        existingAlbum.UserId = album.User;
        if (album.Caps != null)
        {
            existingAlbum.CapLinks =
                album.Caps.Select(capId => new CapToAlbum { AlbumId = id, CapId = capId }).ToList();
        }
        else
        {
            existingAlbum.CapLinks = new List<CapToAlbum>();
        }

        this.context.Albums.Update(existingAlbum);

        await this.context.SaveChangesAsync();

        return new AlbumDto
        {
            Id = existingAlbum.Id,
            Name = existingAlbum.Name,
            Description = existingAlbum.Description,
            Public = existingAlbum.Public,
            User = existingAlbum.UserId,
            Caps = existingAlbum.CapLinks.Select(cl => cl.CapId).ToList()
        };
    }

    public async Task DeleteAlbum(Guid id)
    {
        var album = this.context.Albums.Find(id) ?? throw new ArgumentException("Album not found.");

        this.context.Albums.Remove(album);
        await this.context.SaveChangesAsync();
    }

    public async Task AddCapToAlbum(Guid albumId, Guid capId)
    {
        var album = await this.context.Albums.FindAsync(albumId) ?? throw new ArgumentException("Album not found.");
        var cap = await this.context.Caps.FindAsync(capId) ?? throw new ArgumentException("Cap not found.");

        var capLink = new CapToAlbum { AlbumId = albumId, CapId = capId };

        await this.context.CapToAlbums.AddAsync(capLink);
        await this.context.SaveChangesAsync();
    }

    public async Task RemoveCapFromAlbum(Guid albumId, Guid capId)
    {
        var capLink = await this.context.CapToAlbums
            .Where(cl => cl.AlbumId == albumId && cl.CapId == capId)
            .FirstOrDefaultAsync() ?? throw new ArgumentException("Cap not found in album.");

        this.context.CapToAlbums.Remove(capLink);
        await this.context.SaveChangesAsync();
    }
}
