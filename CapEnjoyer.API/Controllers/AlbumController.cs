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
        var albums = await albumService.GetAllAlbums();
        return Ok(albums);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAlbumById(Guid id)
    {
        var album = await albumService.GetAlbumById(id);
        return Ok(album);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAlbum([FromBody] AlbumInsertDto album)
    {
        var createdAlbum = await albumService.CreateAlbum(album);
        return CreatedAtAction(nameof(GetAlbumById), new { id = createdAlbum.Id }, createdAlbum);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAlbum(Guid id, [FromBody] AlbumInsertDto album)
    {
        var updatedAlbum = await albumService.UpdateAlbum(id, album);
        return Ok(updatedAlbum);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAlbum(Guid id)
    {
        await albumService.DeleteAlbum(id);
        return Ok();
    }

    [HttpPost("{albumId:guid}/caps/{capId:guid}")]
    public async Task<IActionResult> AddCapToAlbum(Guid albumId, Guid capId)
    {
        await albumService.AddCapToAlbum(albumId, capId);
        return Ok();
    }

    [HttpDelete("{albumId:guid}/caps/{capId:guid}")]
    public async Task<IActionResult> RemoveCapFromAlbum(Guid albumId, Guid capId)
    {
        await albumService.RemoveCapFromAlbum(albumId, capId);
        return Ok();
    }
}
