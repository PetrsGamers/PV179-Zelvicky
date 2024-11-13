namespace CapEnjoyer.BL.Controllers;

using DAL;
using DAL.Constants;
using DAL.Entities;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BottleController(CapEnjoyerDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllBottles()
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
                Caps = b.CapLinks.Select(cl => cl.CapId).ToList(),
                IsEditFor = b.IsEditForId
            })
            .ToListAsync();

        return this.Ok(bottles);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBottleById(Guid id)
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
                Caps = b.CapLinks.Select(cl => cl.CapId).ToList(),
                IsEditFor = b.IsEditForId
            })
            .FirstOrDefaultAsync();

        if (bottle == null)
        {
            return this.NotFound($"Bottle with ID {id} not found.");
        }

        return this.Ok(bottle);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBottle(Guid id)
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


    [HttpPost]
    public async Task<IActionResult> CreateBottle([FromBody] BottleDto bottleDto)
    {
        var newId = Guid.NewGuid();
        var bottle = new Bottle
        {
            Id = newId,
            Name = bottleDto.Name,
            Description = bottleDto.Description,
            Voltage = bottleDto.Voltage,
            BottlePicture = bottleDto.BottlePicture,
            DrinkType = Enum.Parse<DrinkType>(bottleDto.DrinkType),
            ProducerId = bottleDto.Producer,
            IsEditForId = bottleDto.IsEditFor
        };

        var capLinks = bottleDto.Caps.Select(capId => new CapToBottle { BottleId = newId, CapId = capId }).ToList();

        await context.Bottles.AddAsync(bottle);
        await context.CapToBottles.AddRangeAsync(capLinks);
        await context.SaveChangesAsync();

        return this.Ok(bottle);
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBottle(Guid id, [FromBody] BottleDto bottleDto)
    {
        var oldBottle = await context.Bottles.FindAsync(id);
        if (oldBottle == null)
        {
            return this.NotFound($"Bottle with ID {id} not found.");
        }

        var newId = Guid.NewGuid();

        var newBottle = new Bottle
        {
            Id = newId,
            Name = bottleDto.Name,
            Description = bottleDto.Description,
            Voltage = bottleDto.Voltage,
            BottlePicture = bottleDto.BottlePicture,
            DrinkType = Enum.Parse<DrinkType>(bottleDto.DrinkType),
            ProducerId = bottleDto.Producer,
            IsEditFor = oldBottle,
            CapLinks = []
        };

        await context.Bottles.AddAsync(newBottle);
        await context.SaveChangesAsync();

        return this.Ok(newBottle);
    }
}
