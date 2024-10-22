using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAL.BL.Controllers;

using DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class AlbumController(ApplicationContext context, ILogger<AlbumController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAlbums()
    {
        try
        {
            var albums = await context.Albums
                .Select(a => new AlbumDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Public = a.Public,
                    User = a.UserId,
                    Caps = a.Caps.Select(c => c.Id).ToList()
                })
                .ToListAsync();

            return this.Ok(albums);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAlbumById(Guid id)
    {
        try
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
                    Caps = a.Caps.Select(c => c.Id).ToList()
                })
                .FirstOrDefaultAsync();

            if (album == null)
            {
                return this.NotFound($"Album with ID {id} not found.");
            }

            return this.Ok(album);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAlbum(Guid id)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }
}
