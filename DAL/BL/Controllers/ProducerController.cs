using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAL.BL.Controllers;

using DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class ProducerController(ApplicationContext context, ILogger<AdminController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducers()
    {
        try
        {
            var producers = await context.Producers
                .Select(p => new ProducerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    City = p.City,
                    Description = p.Description,
                    Country = p.CountryId,
                    IsEditFor = p.IsEditForId
                })
                .ToListAsync();

            return this.Ok(producers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProducerById(Guid id)
    {
        try
        {
            var producer = await context.Producers
                .Where(p => p.Id == id)
                .Select(p => new ProducerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    City = p.City,
                    Description = p.Description,
                    Country = p.CountryId,
                    IsEditFor = p.IsEditForId
                })
                .FirstOrDefaultAsync();

            if (producer == null)
            {
                return this.NotFound($"Producer with ID {id} not found.");
            }

            return this.Ok(producer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProducer(Guid id)
    {
        try
        {
            var producer = await context.Producers.FindAsync(id);
            if (producer == null)
            {
                return this.NotFound($"Producer with ID {id} not found.");
            }

            context.Producers.Remove(producer);
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
