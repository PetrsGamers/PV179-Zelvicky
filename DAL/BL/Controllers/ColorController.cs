using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAL.BL.Controllers;

using DTOs;
using Entities;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class ColorController(ApplicationContext context, ILogger<AdminController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllColors()
    {
        try
        {
            var colors = await context.Colors
                .Select(c => new ColorDto { Id = c.Id, Name = c.Name, HexValue = c.HexCode })
                .ToListAsync();

            return this.Ok(colors);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetColorById(Guid id)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateColor([FromBody] ColorDto colorDto)
    {
        try
        {
            var color = new Color()
            {
                Id = Guid.NewGuid(),
                Name = colorDto.Name,
                HexCode = colorDto.HexValue
            };

            await context.Colors.AddAsync(color);
            await context.SaveChangesAsync();

            return this.Ok(color);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateColor(Guid id, [FromBody] ColorDto colorDto)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteColor(Guid id)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }
}
