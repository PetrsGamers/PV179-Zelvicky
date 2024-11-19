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
        try
        {
            var colors = await colorService.GetColorsAsync();
            return this.Ok(colors);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetColorById(Guid id)
    {
        try
        {
            var color = await colorService.GetColorByIdAsync(id);
            return this.Ok(color);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateColor([FromBody] ColorDto colorDto)
    {
        try
        {
            var color = await colorService.CreateColorAsync(colorDto);
            return this.Ok(color);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateColor(Guid id, [FromBody] ColorDto colorDto)
    {
        try
        {
            var color = await colorService.UpdateColorAsync(id, colorDto);
            return this.Ok(color);
        }
        catch (Exception ex)
        {
            return this.BadRequest(ex.Message);
        }
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteColor(Guid id)
    {
        try
        {
            await colorService.DeleteColorAsync(id);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
