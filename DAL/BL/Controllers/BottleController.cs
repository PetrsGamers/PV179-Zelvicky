namespace DAL.BL.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Constants;
using DTOs;
using Entities;
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
            return this.Ok("Bottle deleted.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBottle([FromBody] BottleDto bottleDto)
    {
        try
        {
            var bottle = new Bottle()
            {
                Id = Guid.NewGuid(),
                Name = bottleDto.Name,
                Description = bottleDto.Description,
                Voltage = bottleDto.Voltage,
                BottlePicture = bottleDto.BottlePicture,
                DrinkType = Enum.Parse<DrinkType>(bottleDto.DrinkType),
                ProducerId = bottleDto.Producer,
                IsEditForId = bottleDto.IsEditFor,
                Caps = bottleDto.Caps.Select(id => new Cap { Id = id }).ToList()
            };

            await context.Bottles.AddAsync(bottle);
            await context.SaveChangesAsync();

            return this.Ok(bottle);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBottle(Guid id, [FromBody] BottleDto bottleDto)
    {
        try
        {
            var bottle = await context.Bottles.FindAsync(id);
            if (bottle == null)
            {
                return this.NotFound($"Bottle with ID {id} not found.");
            }

            bottle.Name = bottleDto.Name;
            bottle.Description = bottleDto.Description;
            bottle.Voltage = bottleDto.Voltage;
            bottle.BottlePicture = bottleDto.BottlePicture;
            bottle.DrinkType = Enum.Parse<DrinkType>(bottleDto.DrinkType);
            bottle.ProducerId = bottleDto.Producer;
            bottle.IsEditForId = bottleDto.IsEditFor;
            bottle.Caps = bottleDto.Caps.Select(cId => new Cap { Id = cId }).ToList();

            context.Bottles.Update(bottle);
            await context.SaveChangesAsync();

            return this.Ok(bottle);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }
}
