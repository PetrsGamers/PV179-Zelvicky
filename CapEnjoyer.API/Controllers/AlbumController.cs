namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AlbumController(IAlbumServiceAsync albumServiceAsync) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAlbums()
    {
        var albums = await albumServiceAsync.GetAllAlbums();
        return this.Ok(albums);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAlbumById(Guid id)
    {
        var album = await albumServiceAsync.GetAlbumById(id);
        return this.Ok(album);

    }

    [HttpPost]
    public async Task<IActionResult> CreateAlbum([FromBody] AlbumInsertDto album)
    {
        var createdAlbum = await albumServiceAsync.CreateAlbum(album);
        return this.CreatedAtAction(nameof(this.GetAlbumById), new { id = createdAlbum.Id }, createdAlbum);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAlbum(Guid id, [FromBody] AlbumInsertDto album)
    {

        var updatedAlbum = await albumServiceAsync.UpdateAlbum(id, album);
        return this.Ok(updatedAlbum);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAlbum(Guid id)
    {

        await albumServiceAsync.DeleteAlbum(id);
        return this.Ok();

    }

    [HttpPost("{albumId:guid}/caps/{capId:guid}")]
    public async Task<IActionResult> AddCapToAlbum(Guid albumId, Guid capId)
    {

        await albumServiceAsync.AddCapToAlbum(albumId, capId);
        return this.Ok();

    }

    [HttpDelete("{albumId:guid}/caps/{capId:guid}")]
    public async Task<IActionResult> RemoveCapFromAlbum(Guid albumId, Guid capId)
    {

        await albumServiceAsync.RemoveCapFromAlbum(albumId, capId);
        return this.Ok();

    }
}
