namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Constants;
using DAL.Entities;
using DTOs;
using Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class BottleService(CapEnjoyerDbContext context, IImageService imageService) : IBottleService
{
    public async Task<BottleDto> GetBottleById(Guid id)
    {
        var bottle = await context.Bottles.Include(b => b.CapLinks)
            .FirstOrDefaultAsync(b => b.Id == id) ?? throw new ArgumentException($"Bottle with {id} not found.");

        return bottle.Adapt<BottleDto>();
    }

    public async Task<IEnumerable<BottleDto>> GetAllBottles()
    {
        var bottles = await context.Bottles
            .Include(b => b.CapLinks)
            .ToListAsync();
        return bottles.Adapt<IEnumerable<BottleDto>>();
    }

    public async Task<BottleDto> CreateBottle(BottleInsertDto bottle)
    {
        if (string.IsNullOrEmpty(bottle.Name) || string.IsNullOrEmpty(bottle.Description))
        {
            throw new ArgumentException("Name or description is missing");
        }

        var producer = await context.Producers.FindAsync(bottle.ProducerId) ??
                       throw new ArgumentException("Producer not found.");
        var id = Guid.NewGuid();
        var newBottle = new Bottle
        {
            Id = id,
            Name = bottle.Name,
            Description = bottle.Description,
            Voltage = bottle.Voltage,
            DrinkType = Enum.Parse<DrinkType>(bottle.DrinkType),
            ProducerId = bottle.ProducerId,
            Producer = producer,
            CapLinks =
            [
                .. context.Caps
                    .Where(c => bottle.Caps != null && bottle.Caps.Contains(c.Id))
                    .Select(c => new CapToBottle { CapId = c.Id, Cap = c, BottleId = id })
            ],
            IsEditForId = bottle.IsEditForId,
            BottlePicture = ""
        };

        await context.Bottles.AddAsync(newBottle);
        await context.SaveChangesAsync();
        if (bottle.BottlePictureFile != null)
        {
            var path = await imageService.UploadImageForBottleAsync(newBottle.Id, bottle.BottlePictureFile);
            newBottle.BottlePicture = path;
        }

        return newBottle.Adapt<BottleDto>();
    }

    public async Task<BottleDto> UpdateBottle(Guid id, BottleInsertDto bottle)
    {
        var existingBottle = await context.Bottles
            .Include(b => b.CapLinks)
            .FirstOrDefaultAsync(b => b.Id == id) ?? throw new ArgumentException($"Bottle with ID {id} not found.");

        existingBottle.Name = bottle.Name;
        existingBottle.Description = bottle.Description;
        existingBottle.Voltage = bottle.Voltage;
        existingBottle.DrinkType = Enum.Parse<DrinkType>(bottle.DrinkType);
        existingBottle.ProducerId = bottle.ProducerId;

        if (bottle.Caps == null)
        {
            existingBottle.CapLinks = [];
        }
        else
        {
            existingBottle.CapLinks = await context.Caps
                .Where(c => bottle.Caps.Contains(c.Id))
                .Select(c => new CapToBottle { BottleId = id, CapId = c.Id, Cap = c, Bottle = existingBottle })
                .ToListAsync();
        }

        context.Bottles.Update(existingBottle);
        await context.SaveChangesAsync();
        if (bottle.BottlePictureFile != null)
        {
            var path = await imageService.UploadImageForBottleAsync(id, bottle.BottlePictureFile);
            existingBottle.BottlePicture = path;
        }

        return existingBottle.Adapt<BottleDto>();
    }

    public async Task DeleteBottle(Guid id)
    {
        var bottle = await context.Bottles.FindAsync(id) ??
                     throw new ArgumentException($"Bottle with ID {id} not found.");

        context.Bottles.Remove(bottle);
        await context.SaveChangesAsync();
    }

    public List<SelectListItem> GetDrinkTypeOptions() =>
        Enum.GetValues(typeof(DrinkType))
            .Cast<DrinkType>()
            .Select(d => new SelectListItem
            {
                Value = d.ToString(),
                Text = d.ToString()
            }).ToList();

    public async Task<List<SelectListItem>> GetBottleOptionsAsync() =>
        await context.Bottles
            .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
            .ToListAsync();
}
