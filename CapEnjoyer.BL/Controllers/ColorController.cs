namespace CapEnjoyer.BL.Controllers;

using DAL;
using DAL.Entities;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ColorController(CapEnjoyerDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllColors()
    {
        var colors = await context.Colors
            .Select(c => new ColorDto { Id = c.Id, Name = c.Name, HexValue = c.HexCode })
            .ToListAsync();

        return this.Ok(colors);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetColorById(Guid id)
    {
        var color = await context.Colors
            .Where(c => c.Id == id)
            .Select(c => new ColorDto { Id = c.Id, Name = c.Name, HexValue = c.HexCode })
            .FirstOrDefaultAsync();

        if (color == null)
        {
            return this.NotFound($"Color with ID {id} not found.");
        }

        return this.Ok(color);
    }

    [HttpPost]
    public async Task<IActionResult> CreateColor([FromBody] ColorDto colorDto)
    {
        var color = new Color { Id = Guid.NewGuid(), Name = colorDto.Name, HexCode = colorDto.HexValue };

        await context.Colors.AddAsync(color);
        await context.SaveChangesAsync();

        return this.Ok(color);
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateColor(Guid id, [FromBody] ColorDto colorDto)
    {
        var color = await context.Colors.FindAsync(id);

        if (color == null)
        {
            return this.NotFound($"Color with ID {id} not found.");
        }

        color.Name = colorDto.Name;
        color.HexCode = colorDto.HexValue;

        context.Colors.Update(color);
        await context.SaveChangesAsync();

        return this.Ok(color);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteColor(Guid id)
    {
        var color = await context.Colors.FindAsync(id);

        if (color == null)
        {
            return this.NotFound($"Color with ID {id} not found.");
        }

        context.Colors.Remove(color);
        await context.SaveChangesAsync();

        return this.Ok("Color has been deleted.");
    }
}
