namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Constants;
using DAL.Entities;
using DTOs;
using Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CapService(CapEnjoyerDbContext context, IImageService imageService) : ICapService
{
    public async Task<CapDto> GetCapByIdAsync(Guid id)
    {
        var cap = await context.Caps
            .Include(c => c.TextColorLinks)
            .Include(c => c.BackgroundColorLinks)
            .Include(c => c.BottleLinks)
            .Include(c => c.AlbumLinks)
            .FirstOrDefaultAsync(c => c.Id == id) ?? throw new ArgumentException($"Cap with ID {id} not found.");

        return cap.Adapt<CapDto>();
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
        await context.AuditLogs.AddAsync(new AuditLog
        {
            Action = AuditLogAction.Delete,
            EditedAt = DateTime.Now.ToUniversalTime(),
            Log = $"Delete cap with {id} ID",
            CapId = id
        });

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CapDto>> GetAllCapsByAlbumIdAsync(Guid albumId)
    {
        var caps = await context.Caps.Include(c => c.TextColorLinks)
            .Include(c => c.BackgroundColorLinks)
            .Include(c => c.BottleLinks)
            .Include(c => c.AlbumLinks)
            .Where(c => c.AlbumLinks.Any(al => al.AlbumId == albumId))
            .ToListAsync();

        return caps.Count == 0 ? [] : caps.Adapt<IEnumerable<CapDto>>();
    }

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

        var caps = await query.Include(c => c.TextColorLinks)
            .Include(c => c.BackgroundColorLinks)
            .Include(c => c.BottleLinks)
            .Include(c => c.AlbumLinks)
            .ToListAsync();

        if (caps.Count == 0)
        {
            throw new ArgumentException("No caps found matching the specified criteria.");
        }

        return caps.Adapt<IEnumerable<CapDto>>();
    }

    public async Task<CapDto> UpdateCapAsync(Guid id, [FromBody] CapInsertDto capInsertDto)
    {
        var oldCap = await context.Caps
                         .Include(c => c.TextColorLinks)
                         .Include(c => c.BackgroundColorLinks)
                         .Include(c => c.BottleLinks)
                         .FirstOrDefaultAsync(c => c.Id == id)
                     ?? throw new ArgumentException($"Cap with ID {id} not found.");

        oldCap.TextOnCap = capInsertDto.TextOnCap;
        oldCap.Description = capInsertDto.Description;

        oldCap.TextColorLinks = await context.Colors
            .Where(tc => capInsertDto.TextColors.Contains(tc.Id))
            .Select(tc => new CapToTextColor { TextColorId = tc.Id, TextColor = tc, Cap = oldCap, CapId = oldCap.Id })
            .ToListAsync();

        oldCap.BackgroundColorLinks = await context.Colors
            .Where(bc => capInsertDto.BgColors.Contains(bc.Id))
            .Select(bc => new CapToBackgroundColor
            {
                BackgroundColor = bc, Cap = oldCap, BackgroundColorId = bc.Id, CapId = oldCap.Id
            })
            .ToListAsync();

        oldCap.BottleLinks = await context.Bottles
            .Where(b => capInsertDto.Bottles.Contains(b.Id))
            .Select(b => new CapToBottle { Bottle = b, Cap = oldCap, BottleId = b.Id, CapId = oldCap.Id })
            .ToListAsync();
        oldCap.IsEditForId = oldCap.Id;

        await context.AuditLogs.AddAsync(new AuditLog
        {
            Action = AuditLogAction.Update,
            EditedAt = DateTime.Now.ToUniversalTime(),
            Log = $"Update cap with {oldCap.Id} ID",
            CapId = oldCap.Id
        });

        await context.SaveChangesAsync();
        if (capInsertDto.CapPictureFile != null)
        {
            var path = await imageService.UploadImageForCapAsync(oldCap.Id, capInsertDto.CapPictureFile);
            oldCap.CapPicture = path;
        }

        return oldCap.Adapt<CapDto>();
    }

    public async Task<CapDto> CreateCapAsync([FromBody] CapInsertDto capInsertDto)
    {
        var cap = new Cap
        {
            Id = Guid.NewGuid(),
            TextOnCap = capInsertDto.TextOnCap,
            Description = capInsertDto.Description,
            TextColorLinks = [],
            BackgroundColorLinks =
                [],
            BottleLinks = [],
            IsEditForId = capInsertDto.IsEditFor,
            CapPicture = ""
        };

        cap.TextColorLinks = await context.Colors
            .Where(tc => capInsertDto.TextColors.Contains(tc.Id))
            .Select(tc => new CapToTextColor { TextColorId = tc.Id, TextColor = tc, Cap = cap, CapId = cap.Id })
            .ToListAsync();

        cap.BackgroundColorLinks = await context.Colors
            .Where(bc => capInsertDto.BgColors.Contains(bc.Id))
            .Select(bc => new CapToBackgroundColor
            {
                BackgroundColor = bc, Cap = cap, BackgroundColorId = bc.Id, CapId = cap.Id
            })
            .ToListAsync();

        cap.BottleLinks = await context.Bottles
            .Where(b => capInsertDto.Bottles.Contains(b.Id))
            .Select(b => new CapToBottle { Bottle = b, Cap = cap, BottleId = b.Id, CapId = cap.Id })
            .ToListAsync();

        await context.Caps.AddAsync(cap);
        await context.AuditLogs.AddAsync(new AuditLog
        {
            Action = AuditLogAction.Create,
            EditedAt = DateTime.Now.ToUniversalTime(),
            Log = $"Create cap with {cap.Id} ID",
            CapId = cap.Id
        });

        await context.SaveChangesAsync();
        if (capInsertDto.CapPictureFile != null)
        {
            var path = await imageService.UploadImageForCapAsync(cap.Id, capInsertDto.CapPictureFile);
            cap.CapPicture = path;
        }

        return cap.Adapt<CapDto>();
    }
}
