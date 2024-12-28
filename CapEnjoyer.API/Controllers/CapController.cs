namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CapController(ICapService capService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCapById(Guid id)
    {
        var cap = await capService.GetCapByIdAsync(id);
        return Ok(cap);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCap(Guid id)
    {
        await capService.DeleteCapAsync(id);
        return Ok($"Cap with ID {id} deleted successfully.");
    }

    [HttpGet("album/{albumId:guid}")]
    public async Task<IActionResult> GetAllCapsByAlbumId(Guid albumId)
    {
        var caps = await capService.GetAllCapsByAlbumIdAsync(albumId);
        return Ok(caps);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCapsFiltered(
        [FromQuery] string? textSubstring = null,
        [FromQuery] List<Guid>? textColorIds = null,
        [FromQuery] List<Guid>? bgColorIds = null,
        [FromQuery] List<Guid>? producerIds = null,
        [FromQuery] List<Guid>? countryIds = null)
    {
        var caps = await capService.GetAllCapsFilteredAsync(textSubstring, textColorIds, bgColorIds, producerIds,
            countryIds);
        return Ok(caps);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCap(CapInsertDto capDto)
    {
        var cap = await capService.CreateCapAsync(capDto);
        return Ok(cap);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCap(Guid id, CapInsertDto capDto)
    {
        var cap = await capService.UpdateCapAsync(id, capDto);
        return Ok(cap);
    }
}
