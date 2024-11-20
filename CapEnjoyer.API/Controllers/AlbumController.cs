namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AlbumController(IAlbumService albumService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAlbums()
    {
        try
        {
            var albums = await albumService.GetAllAlbums();
            return this.Ok(albums);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAlbumById(Guid id)
    {
        try
        {
            var album = await albumService.GetAlbumById(id);

            return this.Ok(album);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAlbum([FromBody] AlbumInsertDto album)
    {
        try
        {
            var createdAlbum = await albumService.CreateAlbum(album);


            return this.CreatedAtAction(nameof(this.GetAlbumById), new { id = createdAlbum.Id }, createdAlbum);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAlbum(Guid id, [FromBody] AlbumInsertDto album)
    {
        try
        {
            var updatedAlbum = await albumService.UpdateAlbum(id, album);


            return this.Ok(updatedAlbum);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAlbum(Guid id)
    {
        try
        {
            await albumService.DeleteAlbum(id);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPost("{albumId:guid}/caps/{capId:guid}")]
    public async Task<IActionResult> AddCapToAlbum(Guid albumId, Guid capId)
    {
        try
        {
            await albumService.AddCapToAlbum(albumId, capId);

            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpDelete("{albumId:guid}/caps/{capId:guid}")]
    public async Task<IActionResult> RemoveCapFromAlbum(Guid albumId, Guid capId)
    {
        try
        {
            await albumService.RemoveCapFromAlbum(albumId, capId);

            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
