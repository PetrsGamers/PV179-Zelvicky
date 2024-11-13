namespace CapEnjoyer.BL.Controllers;

using DAL;
using DAL.Entities;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AlbumController(CapEnjoyerDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAlbums()
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

        return this.Ok(albums);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAlbumById(Guid id)
    {
        var album = await context.Albums
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
            .FirstOrDefaultAsync();

        if (album == null)
        {
            return this.NotFound($"Album with ID {id} not found.");
        }

        return this.Ok(album);
    }


    [HttpPost]
    public async Task<IActionResult> CreateAlbum([FromBody] AlbumInsertDto albumInsertDto)
    {
        var newId = Guid.NewGuid();
        var album = new Album
        {
            Id = newId,
            Name = albumInsertDto.Name,
            Description = albumInsertDto.Description,
            Public = albumInsertDto.Public,
            UserId = albumInsertDto.User
        };

        var capLinks = albumInsertDto.Caps.Select(capId => new CapToAlbum { AlbumId = newId, CapId = capId }).ToList();


        await context.Albums.AddAsync(album);
        await context.CapToAlbums.AddRangeAsync(capLinks);
        await context.SaveChangesAsync();

        return this.Ok(album);
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAlbum(Guid id, [FromBody] AlbumDto albumDto)
    {
        var existingAlbum = await context.Albums.FindAsync(id);
        if (existingAlbum == null)
        {
            return this.NotFound($"Album with ID {id} not found.");
        }

        existingAlbum.Name = albumDto.Name;
        existingAlbum.Description = albumDto.Description;
        existingAlbum.Public = albumDto.Public;
        existingAlbum.UserId = albumDto.User;

        context.Albums.Update(existingAlbum);
        await context.SaveChangesAsync();

        return this.Ok(existingAlbum);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAlbum(Guid id)
    {
        var album = await context.Albums.FindAsync(id);
        if (album == null)
        {
            return this.NotFound($"Album with ID {id} not found.");
        }

        context.Albums.Remove(album);
        await context.SaveChangesAsync();
        return this.NoContent();
    }
}
