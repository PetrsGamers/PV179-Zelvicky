namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ColorController(IColorService colorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllColors()
    {
        var colors = await colorService.GetColorsAsync();
        return Ok(colors);
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
