namespace DAL.BL.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using DTOs;
using Entities;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class CapController(ApplicationContext context, ILogger<CapController> logger) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCapById(Guid id)
    {
        try
        {
            var cap = await context.Caps
                .Where(c => c.Id == id)
                .Select(c => new CapDto
                {
                    Id = c.Id,
                    TextOnCap = c.TextOnCap,
                    Description = c.Description,
                    CapPicture = c.CapPicture,
                    TextColors = c.TextColors.Select(tc => tc.Id).ToList(),
                    BgColors = c.BgColors.Select(bc => bc.Id).ToList(),
                    Bottles = c.Bottles.Select(b => b.Id).ToList(),
                    IsEditFor = c.IsEditForId
                })
                .FirstOrDefaultAsync();

            if (cap == null)
            {
                return this.NotFound($"Cap with ID {id} not found.");
            }

            return this.Ok(cap);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCap(Guid id)
    {
        try
        {
            var cap = await context.Caps.FindAsync(id);
            if (cap == null)
            {
                return this.NotFound($"Cap with ID {id} not found.");
            }

            context.Caps.Remove(cap);
            await context.SaveChangesAsync();
            return this.NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("album/{albumId:guid}")]
    public async Task<IActionResult> GetAllCapsByAlbumId(Guid albumId)
    {
        try
        {
            var caps = await context.Caps
                .Where(c => c.Albums.Any(a => a.Id == albumId))
                .Select(c => new CapDto
                {
                    Id = c.Id,
                    TextOnCap = c.TextOnCap,
                    Description = c.Description,
                    CapPicture = c.CapPicture,
                    TextColors = c.TextColors.Select(tc => tc.Id).ToList(),
                    BgColors = c.BgColors.Select(bc => bc.Id).ToList(),
                    Bottles = c.Bottles.Select(b => b.Id).ToList(),
                    IsEditFor = c.IsEditForId
                })
                .ToListAsync();

            if (caps.Count == 0)
            {
                return this.NotFound($"No caps found for Album with ID {albumId}.");
            }

            return this.Ok(caps);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCapsFiltered(
       [FromQuery] string? textSubstring = null,
       [FromQuery] List<Guid>? textColorIds = null,
       [FromQuery] List<Guid>? bgColorIds = null,
       [FromQuery] List<Guid>? producerIds = null,
       [FromQuery] List<Guid>? countryIds = null)
    {
        try
        {
            var query = context.Caps.AsQueryable();

            if (!string.IsNullOrWhiteSpace(textSubstring))
            {
                query = query.Where(c => c.TextOnCap.Contains(textSubstring));
            }

            if (textColorIds != null && textColorIds.Count != 0)
            {
                query = query.Where(c => textColorIds.All(tcId => c.TextColors.Any(tc => tc.Id == tcId)));
            }

            if (bgColorIds != null && bgColorIds.Count != 0)
            {
                query = query.Where(c => bgColorIds.All(bcId => c.BgColors.Any(bc => bc.Id == bcId)));
            }

            if (producerIds != null && producerIds.Count != 0)
            {
                query = query.Where(c => c.Bottles.Any(b => producerIds.Contains(b.ProducerId)));
            }

            if (countryIds != null && countryIds.Count != 0)
            {
                query = query.Where(c => c.Bottles.Any(b => countryIds.Contains(b.Producer.CountryId)));
            }

            var caps = await query
                .Select(c => new CapDto
                {
                    Id = c.Id,
                    TextOnCap = c.TextOnCap,
                    Description = c.Description,
                    CapPicture = c.CapPicture,
                    TextColors = c.TextColors.Select(tc => tc.Id).ToList(),
                    BgColors = c.BgColors.Select(bc => bc.Id).ToList(),
                    Bottles = c.Bottles.Select(b => b.Id).ToList(),
                    IsEditFor = c.IsEditForId
                })
                .ToListAsync();

            if (caps.Count == 0)
            {
                return this.NotFound("No caps found matching the specified criteria.");
            }

            return this.Ok(caps);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCap([FromBody] CapDto capDto)
    {
        try
        {
            var cap = new Cap()
            {
                Id = Guid.NewGuid(),
                TextOnCap = capDto.TextOnCap,
                Description = capDto.Description,
                CapPicture = capDto.CapPicture,
                TextColors = capDto.TextColors.Select(id => new Color { Id = id }).ToList(),
                BgColors = capDto.BgColors.Select(id => new Color { Id = id }).ToList(),
                Bottles = capDto.Bottles.Select(id => new Bottle { Id = id }).ToList(),
                IsEditForId = capDto.IsEditFor
            };

            await context.Caps.AddAsync(cap);
            await context.SaveChangesAsync();

            return this.Ok(cap);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCap(Guid id, [FromBody] CapDto capDto)
    {
        try
        {
            var cap = await context.Caps.Include(c => c.TextColors)
                .Include(c => c.BgColors)
                .Include(c => c.Bottles)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cap == null)
            {
                return this.NotFound($"Cap with ID {id} not found.");
            }

            cap.TextOnCap = capDto.TextOnCap;
            cap.Description = capDto.Description;
            cap.CapPicture = capDto.CapPicture;
            cap.TextColors = capDto.TextColors.Select(id => new Color { Id = id }).ToList();
            cap.BgColors = capDto.BgColors.Select(id => new Color { Id = id }).ToList();
            cap.Bottles = capDto.Bottles.Select(id => new Bottle { Id = id }).ToList();
            cap.IsEditForId = capDto.IsEditFor;

            context.Caps.Update(cap);
            await context.SaveChangesAsync();

            return this.Ok(cap);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }


}
