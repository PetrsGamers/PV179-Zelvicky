namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Route("api/[controller]")]
public class ColorController(IColorService colorService, IMemoryCache memoryCache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllColors()
    {
        const string cacheKey = "AllColors";
        var cachedColors = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await colorService.GetColorsAsync();
        });
        return Ok(cachedColors);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetColorById(Guid id)
    {
        var color = await colorService.GetColorByIdAsync(id);
        return Ok(color);
    }

    [HttpPost]
    public async Task<IActionResult> CreateColor([FromBody] ColorInsertDto colorDto)
    {
        var color = await colorService.CreateColorAsync(colorDto);
        return Ok(color);
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateColor(Guid id, [FromBody] ColorInsertDto colorDto)
    {
        var color = await colorService.UpdateColorAsync(id, colorDto);
        return Ok(color);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteColor(Guid id)
    {
        await colorService.DeleteColorAsync(id);
        return Ok();
    }
}
