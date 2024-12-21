namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Entities;
using DTOs;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class AlbumService(CapEnjoyerDbContext context) : IAlbumServiceAsync
{

    public async Task<IEnumerable<AlbumDto>> GetAllAlbums()
    {
        var albums = await context.Albums
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
        var album = await context.Albums
            .Select(a => new AlbumDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Public = a.Public,
                User = a.UserId,
                Caps = a.CapLinks.Select(cl => cl.CapId).ToList()
            })
            .FirstOrDefaultAsync(a => a.Id == id) ?? throw new ArgumentException($"Album with ID {id} not found.");

        return album;
    }


    public async Task<AlbumDto> CreateAlbum(AlbumInsertDto album)
    {
        if (string.IsNullOrEmpty(album.Name) || string.IsNullOrEmpty(album.Description))
        {
            throw new ArgumentException("Name and Description are required.");
        }

        var user = await context.Users.FindAsync(album.User) ?? throw new ArgumentException("User not found.");

        var newId = Guid.NewGuid();
        var newAlbum = new Album
        {
            Id = newId,
            Name = album.Name,
            Description = album.Description,
            Public = album.Public,
            UserId = user.Id,
            User = user
        };
        var capLinks = new List<CapToAlbum>();
        if (album.Caps != null)
        {
            capLinks = await context.Caps
                .Where(c => album.Caps.Contains(c.Id))
                .Select(c => new CapToAlbum
                {
                    AlbumId = newAlbum.Id,
                    CapId = c.Id,
                    Cap = c,
                    Album = newAlbum
                })
                .ToListAsync();
        }
        await context.Albums.AddAsync(newAlbum);
        await context.CapToAlbums.AddRangeAsync(capLinks);
        await context.SaveChangesAsync();

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
        var existingAlbum =
            await context.Albums.Include(a => a.CapLinks).FirstOrDefaultAsync(a => a.Id == id) ??
            throw new ArgumentException("Album not found.");

        existingAlbum.Name = album.Name;
        existingAlbum.Description = album.Description;
        existingAlbum.Public = album.Public;
        existingAlbum.UserId = album.User;
        if (album.Caps == null)
        {
            existingAlbum.CapLinks = [];
        }
        else
        {
            existingAlbum.CapLinks = await context.Caps
                .Where(c => album.Caps.Contains(c.Id))
                .Select(c => new CapToAlbum
                {
                    AlbumId = id,
                    CapId = c.Id,
                    Cap = c,
                    Album = existingAlbum
                })
                .ToListAsync();
        }

        context.Albums.Update(existingAlbum);

        await context.SaveChangesAsync();

        return new AlbumDto
        {
            Id = existingAlbum.Id,
            Name = existingAlbum.Name,
            Description = existingAlbum.Description,
            Public = existingAlbum.Public,
            User = existingAlbum.UserId,
        };
    }

    public async Task DeleteAlbum(Guid id)
    {
        var album = await context.Albums.FindAsync(id) ?? throw new ArgumentException("Album not found.");
        context.Albums.Remove(album);
        await context.SaveChangesAsync();
    }

    public async Task AddCapToAlbum(Guid albumId, Guid capId)
    {
        var album = await context.Albums.FindAsync(albumId) ?? throw new ArgumentException("Album not found.");
        var cap = await context.Caps.FindAsync(capId) ?? throw new ArgumentException("Cap not found.");

        var capLink = new CapToAlbum { AlbumId = albumId, CapId = capId, Cap = cap, Album = album };

        await context.CapToAlbums.AddAsync(capLink);
        await context.SaveChangesAsync();
    }

    public async Task RemoveCapFromAlbum(Guid albumId, Guid capId)
    {
        var capLink = await context.CapToAlbums
            .Where(cl => cl.AlbumId == albumId && cl.CapId == capId)
            .FirstOrDefaultAsync() ?? throw new ArgumentException("Cap not found in album.");

        context.CapToAlbums.Remove(capLink);
        await context.SaveChangesAsync();
    }
}
