namespace CapEnjoyer.BL.Controllers;

using DAL;
using DAL.Entities;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CapController(CapEnjoyerDbContext context, ILogger<CapController> logger) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCapById(Guid id)
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
            return this.NotFound($"Cap with ID {id} not found.");
        }

        return this.Ok(cap);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCap(Guid id)
    {
        var cap = await context.Caps
            .Include(c => c.TextColorLinks)
            .Include(c => c.BackgroundColorLinks)
            .Include(c => c.BottleLinks)
            .Include(c => c.AlbumLinks)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cap == null)
        {
            return this.NotFound($"Cap with ID {id} not found.");
        }

        context.CapToTextColors.RemoveRange(cap.TextColorLinks);
        context.CapToBackgroundColors.RemoveRange(cap.BackgroundColorLinks);
        context.CapToBottles.RemoveRange(cap.BottleLinks);
        context.CapToAlbums.RemoveRange(cap.AlbumLinks);

        context.Caps.Remove(cap);
        await context.SaveChangesAsync();
        return this.Ok();
    }

    [HttpGet("album/{albumId:guid}")]
    public async Task<IActionResult> GetAllCapsByAlbumId(Guid albumId)
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

        if (caps.Count == 0)
        {
            return this.NotFound($"No caps found for Album with ID {albumId}.");
        }

        return this.Ok(caps);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCapsFiltered(
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
            return this.NotFound("No caps found matching the specified criteria.");
        }

        return this.Ok(caps);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCap([FromBody] CapDto capDto)
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

        return this.Ok(cap);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCap(Guid id, [FromBody] CapDto capDto)
    {
        var oldCap = await context.Caps
            .Include(c => c.TextColorLinks)
            .Include(c => c.BackgroundColorLinks)
            .Include(c => c.BottleLinks)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (oldCap == null)
        {
            return this.NotFound($"Cap with ID {id} not found.");
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

        return this.Ok(newCap);
    }
}
