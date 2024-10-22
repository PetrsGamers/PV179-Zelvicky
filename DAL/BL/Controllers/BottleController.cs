using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAL.BL.Controllers;

using DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class BottleController(ApplicationContext context, ILogger<BottleController> logger) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAllBottles()
    {
        try
        {
            var bottles = await context.Bottles
                .Select(b => new BottleDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Voltage = b.Voltage,
                    BottlePicture = b.BottlePicture,
                    DrinkType = b.DrinkType.ToString(),
                    Producer = b.ProducerId,
                    Caps = b.Caps.Select(c => c.Id).ToList(),
                    IsEditFor = b.IsEditForId
                })
                .ToListAsync();

            return this.Ok(bottles);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBottleById(Guid id)
    {
        try
        {
        var bottle = await context.Bottles
            .Where(b => b.Id == id)
            .Select(b => new BottleDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Voltage = b.Voltage,
                BottlePicture = b.BottlePicture,
                DrinkType = b.DrinkType.ToString(),
                Producer = b.ProducerId,
                Caps = b.Caps.Select(c => c.Id).ToList(),
                IsEditFor = b.IsEditForId
            })
            .FirstOrDefaultAsync();

        if (bottle == null)
        {
            return this.NotFound($"Bottle with ID {id} not found.");
        }

        return this.Ok(bottle);

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBottle(Guid id)
    {
        try
        {
            var bottle = await context.Bottles.FindAsync(id);
            if (bottle == null)
            {
                return this.NotFound($"Bottle with ID {id} not found.");
            }

            context.Bottles.Remove(bottle);
            await context.SaveChangesAsync();
            return this.NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }
}
