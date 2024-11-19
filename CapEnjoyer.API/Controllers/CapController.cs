namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CapController(ICapService capService) : ControllerBase
{
    [HttpPost("upload-image/{capId:guid}")]
    public async Task<IActionResult> UploadImageForCap(Guid capId, IFormFile image)
    {
        try
        {
            await capService.UploadImageForCapAsync(capId, image);
            return this.Ok("Image uploaded successfully.");
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCapById(Guid id)
    {
        try
        {
            var cap = await capService.GetCapByIdAsync(id);
            return this.Ok(cap);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCap(Guid id)
    {
        try
        {
            await capService.DeleteCapAsync(id);
            return this.Ok($"Cap with ID {id} deleted successfully.");
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpGet("album/{albumId:guid}")]
    public async Task<IActionResult> GetAllCapsByAlbumId(Guid albumId)
    {
        try
        {
            var caps = await capService.GetAllCapsByAlbumIdAsync(albumId);
            return this.Ok(caps);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCapsFiltered(
        [FromQuery] string? textSubstring = null,
        [FromQuery] List<Guid>? textColorIds = null,
        [FromQuery] List<Guid>? bgColorIds = null,
        [FromQuery] List<Guid>? producerIds = null,
        [FromQuery] List<Guid>? countryIds = null)
    {
        try
        {
            var caps = await capService.GetAllCapsFilteredAsync(textSubstring, textColorIds, bgColorIds, producerIds,
                countryIds);
            return this.Ok(caps);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCap([FromBody] CapDto capDto)
    {
        try
        {
            var cap = await capService.CreateCapAsync(capDto);
            return this.Ok(cap);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCap(Guid id, [FromBody] CapDto capDto)
    {
        try
        {
            var cap = await capService.UpdateCapAsync(id, capDto);
            return this.Ok(cap);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
