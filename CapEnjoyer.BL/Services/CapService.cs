namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Entities;
using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CapService(CapEnjoyerDbContext context) : ICapService
{
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
            .FirstOrDefaultAsync();

        if (cap == null)
        {
            throw new ArgumentException($"Cap with ID {id} not found.");
        }

        return cap;
    }

    public async Task DeleteCapAsync(Guid id)
    {
        var cap = await context.Caps
            .Include(c => c.TextColorLinks)
            .Include(c => c.BackgroundColorLinks)
            .Include(c => c.BottleLinks)
            .Include(c => c.AlbumLinks)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cap == null)
        {
            throw new ArgumentException($"Cap with ID {id} not found.");
        }

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

    [HttpPost]
    public async Task<CapDto> CreateCapAsync([FromBody] CapDto capDto)
    {
        var cap = new Cap
        {
            Id = Guid.NewGuid(),
            TextOnCap = capDto.TextOnCap,
            Description = capDto.Description,
            CapPicture = capDto.CapPicture,
            TextColorLinks = capDto.TextColors.Select(id => new CapToTextColor { TextColorId = id }).ToList(),
            BackgroundColorLinks =
                capDto.BgColors.Select(id => new CapToBackgroundColor { BackgroundColorId = id }).ToList(),
            BottleLinks = capDto.Bottles.Select(id => new CapToBottle { BottleId = id }).ToList(),
            IsEditForId = capDto.IsEditFor
        };

        await context.Caps.AddAsync(cap);
        await context.SaveChangesAsync();

        return capDto;
    }

    [HttpPut("{id:guid}")]
    public async Task<CapDto> UpdateCapAsync(Guid id, [FromBody] CapDto capDto)
    {
        var oldCap = await context.Caps
            .Include(c => c.TextColorLinks)
            .Include(c => c.BackgroundColorLinks)
            .Include(c => c.BottleLinks)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (oldCap == null)
        {
            throw new ArgumentException($"Cap with ID {id} not found.");
        }

        var newCap = new Cap
        {
            Id = Guid.NewGuid(),
            TextOnCap = capDto.TextOnCap,
            Description = capDto.Description,
            CapPicture = capDto.CapPicture,
            TextColorLinks = capDto.TextColors.Select(colorId => new CapToTextColor { TextColorId = colorId }).ToList(),
            BackgroundColorLinks =
                capDto.BgColors.Select(colorId => new CapToBackgroundColor { BackgroundColorId = colorId }).ToList(),
            BottleLinks = capDto.Bottles.Select(bottleId => new CapToBottle { BottleId = bottleId }).ToList(),
            IsEditForId = oldCap.Id
        };

        await context.Caps.AddAsync(newCap);
        await context.SaveChangesAsync();

        return capDto;
    }
}
