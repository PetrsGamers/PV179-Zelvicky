namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Entities;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CapService(CapEnjoyerDbContext context) : ICapService
{
    public async Task UploadImageForCapAsync(Guid capId, IFormFile image)
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
        var uploadsFolder = Path.Combine(baseDirectory, @"wwwroot\images\caps");
        Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

        var fileName = $"{Guid.NewGuid()}_cap_{image.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var cap = await context.Caps.FirstOrDefaultAsync(c => c.Id == capId) ??
                  throw new ArgumentException($"Cap with ID {capId} not found.");
        cap.CapPicture = filePath;

        context.Caps.Update(cap);
        await context.SaveChangesAsync();
    }

    public async Task<CapDto> GetCapByIdAsync(Guid id)
    {
        var cap = await context.Caps
            .Where(c => c.Id == id)
            .Select(c => new CapDto
            {
                Id = c.Id,
                TextOnCap = c.TextOnCap,
                Description = c.Description,
                CapPicture = c.CapPicture,
                TextColors = c.TextColorLinks.Select(tc => tc.TextColorId).ToList(),
                BgColors = c.BackgroundColorLinks.Select(bc => bc.BackgroundColorId).ToList(),
                Bottles = c.BottleLinks.Select(bl => bl.BottleId).ToList(),
                IsEditFor = c.IsEditForId
            })
            .FirstOrDefaultAsync() ?? throw new ArgumentException($"Cap with ID {id} not found.");

        return cap;
    }

    public async Task DeleteCapAsync(Guid id)
    {
        var cap = await context.Caps
            .Include(c => c.TextColorLinks)
            .Include(c => c.BackgroundColorLinks)
            .Include(c => c.BottleLinks)
            .Include(c => c.AlbumLinks)
            .FirstOrDefaultAsync(c => c.Id == id) ?? throw new ArgumentException($"Cap with ID {id} not found.");

        context.CapToTextColors.RemoveRange(cap.TextColorLinks);
        context.CapToBackgroundColors.RemoveRange(cap.BackgroundColorLinks);
        context.CapToBottles.RemoveRange(cap.BottleLinks);
        context.CapToAlbums.RemoveRange(cap.AlbumLinks);

        context.Caps.Remove(cap);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CapDto>> GetAllCapsByAlbumIdAsync(Guid albumId)
    {
        var caps = await context.Caps
            .Where(c => c.AlbumLinks.Any(al => al.AlbumId == albumId))
            .Select(c => new CapDto
            {
                Id = c.Id,
                TextOnCap = c.TextOnCap,
                Description = c.Description,
                CapPicture = c.CapPicture,
                TextColors = c.TextColorLinks.Select(tc => tc.TextColorId).ToList(),
                BgColors = c.BackgroundColorLinks.Select(bc => bc.BackgroundColorId).ToList(),
                Bottles = c.BottleLinks.Select(bl => bl.BottleId).ToList(),
                IsEditFor = c.IsEditForId
            })
            .ToListAsync();

        return caps.Count == 0 ? [] : caps;
    }

    [HttpGet]
    public async Task<IEnumerable<CapDto>> GetAllCapsFilteredAsync(
        [FromQuery] string? textSubstring = null,
        [FromQuery] List<Guid>? textColorIds = null,
        [FromQuery] List<Guid>? bgColorIds = null,
        [FromQuery] List<Guid>? producerIds = null,
        [FromQuery] List<Guid>? countryIds = null)
    {
        var query = context.Caps.AsQueryable();

        if (!string.IsNullOrWhiteSpace(textSubstring))
        {
            query = query.Where(c => c.TextOnCap.Contains(textSubstring));
        }

        if (textColorIds != null && textColorIds.Count != 0)
        {
            query = query.Where(c => textColorIds.All(tcId => c.TextColorLinks.Any(tc => tc.TextColorId == tcId)));
        }

        if (bgColorIds != null && bgColorIds.Count != 0)
        {
            query = query.Where(c =>
                bgColorIds.All(bcId => c.BackgroundColorLinks.Any(bc => bc.BackgroundColorId == bcId)));
        }

        if (producerIds != null && producerIds.Count != 0)
        {
            query = query.Where(c => c.BottleLinks.Any(b => producerIds.Contains(b.Bottle.ProducerId)));
        }

        if (countryIds != null && countryIds.Count != 0)
        {
            query = query.Where(c => c.BottleLinks.Any(b => countryIds.Contains(b.Bottle.Producer.CountryId)));
        }

        var caps = await query
            .Select(c => new CapDto
            {
                Id = c.Id,
                TextOnCap = c.TextOnCap,
                Description = c.Description,
                CapPicture = c.CapPicture,
                TextColors = c.TextColorLinks.Select(tc => tc.TextColorId).ToList(),
                BgColors = c.BackgroundColorLinks.Select(bc => bc.BackgroundColorId).ToList(),
                Bottles = c.BottleLinks.Select(bl => bl.BottleId).ToList(),
                IsEditFor = c.IsEditForId
            })
            .ToListAsync();

        if (caps.Count == 0)
        {
            throw new ArgumentException("No caps found matching the specified criteria.");
        }

        return caps;
    }

    [HttpPut("{id:guid}")]
    public async Task<CapDto> UpdateCapAsync(Guid id, [FromBody] CapInsertDto capInsertDto)
    {
        // Retrieve the existing entity
        var oldCap = await context.Caps
                         .Include(c => c.TextColorLinks)
                         .Include(c => c.BackgroundColorLinks)
                         .Include(c => c.BottleLinks)
                         .FirstOrDefaultAsync(c => c.Id == id)
                     ?? throw new ArgumentException($"Cap with ID {id} not found.");

        oldCap.TextOnCap = capInsertDto.TextOnCap;
        oldCap.Description = capInsertDto.Description;
        oldCap.CapPicture = capInsertDto.CapPicture;

        oldCap.TextColorLinks = capInsertDto.TextColors
            .Select(colorId => new CapToTextColor { TextColorId = colorId }).ToList();
        oldCap.BackgroundColorLinks = capInsertDto.BgColors
            .Select(colorId => new CapToBackgroundColor { BackgroundColorId = colorId }).ToList();
        oldCap.BottleLinks = capInsertDto.Bottles
            .Select(bottleId => new CapToBottle { BottleId = bottleId }).ToList();
        oldCap.IsEditForId = oldCap.Id;

        await context.SaveChangesAsync();
        return new CapDto
        {
            Id = oldCap.Id,
            TextOnCap = oldCap.TextOnCap,
            Description = oldCap.Description,
            CapPicture = oldCap.CapPicture,
            TextColors = oldCap.TextColorLinks.Select(tc => tc.TextColorId).ToList(),
            BgColors = oldCap.BackgroundColorLinks.Select(bc => bc.BackgroundColorId).ToList(),
            Bottles = oldCap.BottleLinks.Select(bl => bl.BottleId).ToList(),
            IsEditFor = oldCap.IsEditForId
        };
    }

    [HttpPost]
    public async Task<CapDto> CreateCapAsync([FromBody] CapInsertDto capInsertDto)
    {
        var cap = new Cap
        {
            Id = Guid.NewGuid(),
            TextOnCap = capInsertDto.TextOnCap,
            Description = capInsertDto.Description,
            CapPicture = capInsertDto.CapPicture,
            TextColorLinks = capInsertDto.TextColors.Select(id => new CapToTextColor { TextColorId = id }).ToList(),
            BackgroundColorLinks =
                capInsertDto.BgColors.Select(id => new CapToBackgroundColor { BackgroundColorId = id }).ToList(),
            BottleLinks = capInsertDto.Bottles.Select(id => new CapToBottle { BottleId = id }).ToList(),
            IsEditForId = capInsertDto.IsEditFor
        };

        await context.Caps.AddAsync(cap);
        await context.SaveChangesAsync();

        return new CapDto
        {
            Id = cap.Id,
            TextOnCap = cap.TextOnCap,
            Description = cap.Description,
            CapPicture = cap.CapPicture,
            TextColors = cap.TextColorLinks.Select(tc => tc.TextColorId).ToList(),
            BgColors = cap.BackgroundColorLinks.Select(bc => bc.BackgroundColorId).ToList(),
            Bottles = cap.BottleLinks.Select(bl => bl.BottleId).ToList(),
            IsEditFor = cap.IsEditForId
        };
    }
}
