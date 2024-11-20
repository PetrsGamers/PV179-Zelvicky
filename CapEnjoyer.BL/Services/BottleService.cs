namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Constants;
using DAL.Entities;
using DTOs;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class BottleService(CapEnjoyerDbContext context) : IBottleService
{
    public async Task UploadImageForBottleAsync(Guid bottleId, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Invalid file.");
        }

        if (image.Length > 5 * 2048 * 2048)
        {
            throw new ArgumentException("File size is too big.");
        }

        if (image.ContentType is not "image/jpeg" and not "image/png")
        {
            throw new ArgumentException("Invalid file type.");
        }

        var baseDirectory = Directory.GetCurrentDirectory();
        var uploadsFolder = Path.Combine(baseDirectory, @"wwwroot\images\bottles");
        Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

        var fileName = $"{Guid.NewGuid()}_bottle_{image.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var bottle = await context.Bottles.FirstOrDefaultAsync(b => b.Id == bottleId) ??
                     throw new ArgumentException($"Bottle with ID {bottleId} not found.");
        bottle.BottlePicture = filePath;

        context.Bottles.Update(bottle);
        await context.SaveChangesAsync();
    }

    public async Task<BottleDto> GetBottleById(Guid id)
    {
        var bottle = await context.Bottles
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
            .FirstOrDefaultAsync(b => b.Id == id) ?? throw new ArgumentException($"Bottle with {id} not found.");

        return bottle;
    }

    public async Task<IEnumerable<BottleDto>> GetAllBottles()
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
        return bottles;
    }

    public async Task<BottleDto> CreateBottle(BottleDto bottle)
    {
        // Přidat logiku pro validaci nebo další procesy
        if (string.IsNullOrEmpty(bottle.Name) || string.IsNullOrEmpty(bottle.Description))
        {
            throw new ArgumentException("Name or destription is missing");
        }

        var newBottle = new Bottle
        {
            Id = Guid.NewGuid(),
            Name = bottle.Name,
            Description = bottle.Description,
            Voltage = bottle.Voltage,
            BottlePicture = bottle.BottlePicture,
            DrinkType = Enum.Parse<DrinkType>(bottle.DrinkType),
            ProducerId = bottle.Producer,
            CapLinks = bottle.Caps.Select(capId => new CapToBottle { CapId = capId }).ToList(),
            IsEditForId = bottle.IsEditFor
        };

        await context.Bottles.AddAsync(newBottle);
        await context.SaveChangesAsync();

        return new BottleDto
        {
            Id = newBottle.Id,
            Name = newBottle.Name,
            Description = newBottle.Description,
            Voltage = newBottle.Voltage,
            BottlePicture = newBottle.BottlePicture,
            DrinkType = newBottle.DrinkType.ToString(),
            Producer = newBottle.ProducerId,
            Caps = newBottle.CapLinks.Select(cl => cl.CapId).ToList(),
            IsEditFor = newBottle.IsEditForId
        };
    }

    public async Task<BottleDto> UpdateBottle(Guid id, BottleDto bottle)
    {
        var existingBottle = await context.Bottles
            .Include(b => b.CapLinks)
            .FirstOrDefaultAsync(b => b.Id == id) ?? throw new ArgumentException($"Botte with ID {id} not found.");

        existingBottle.Name = bottle.Name;
        existingBottle.Description = bottle.Description;
        existingBottle.Voltage = bottle.Voltage;
        existingBottle.BottlePicture = bottle.BottlePicture;
        existingBottle.DrinkType = Enum.Parse<DrinkType>(bottle.DrinkType);
        existingBottle.ProducerId = bottle.Producer;
        existingBottle.CapLinks = bottle.Caps.Select(capId => new CapToBottle { CapId = capId }).ToList();
        existingBottle.IsEditForId = bottle.IsEditFor;

        context.Bottles.Update(existingBottle);
        await context.SaveChangesAsync();

        return new BottleDto
        {
            Id = existingBottle.Id,
            Name = existingBottle.Name,
            Description = existingBottle.Description,
            Voltage = existingBottle.Voltage,
            BottlePicture = existingBottle.BottlePicture,
            DrinkType = existingBottle.DrinkType.ToString(),
            Producer = existingBottle.ProducerId,
            Caps = existingBottle.CapLinks.Select(cl => cl.CapId).ToList(),
            IsEditFor = existingBottle.IsEditForId
        };
    }

    public async Task DeleteBottle(Guid id)
    {
        var bottle = await context.Bottles.FindAsync(id) ??
                     throw new ArgumentException($"Bottle with ID {id} not found.");

        context.Bottles.Remove(bottle);
        await context.SaveChangesAsync();
    }
}
