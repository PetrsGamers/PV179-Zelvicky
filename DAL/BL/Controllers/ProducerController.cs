namespace DAL.BL.Controllers;
using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class ProducerController(ApplicationContext context, ILogger<ProducerController> logger) : ControllerBase
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

    [HttpPost]
    public async Task<IActionResult> CreateProducer([FromBody] ProducerInsertDto producerDto)
    {
        try
        {
            if (producerDto.Name.Length < 1 && producerDto.City.Length < 1)
            {
                return this.BadRequest($"Name and city must be non-empty string.");
            }
            var country = await context.Countries.FindAsync(producerDto.Country);
            if (country == null)
            {
                return this.NotFound($"Country with ID {producerDto.Country} not found.");
            }
            if (producerDto.IsEditFor != null)
            {
                var isEditFor = await context.Producers.FindAsync(producerDto.IsEditFor);
                if (isEditFor == null)
                {
                    return this.NotFound($"Producer with ID {producerDto.Country} not found.");
                }
            }
            var producer = new Producer
            {
                Id = Guid.NewGuid(),
                Name = producerDto.Name,
                City = producerDto.City,
                Description = producerDto.Description,
                CountryId = producerDto.Country,
                IsEditForId = producerDto.IsEditFor
            };

            await context.Producers.AddAsync(producer);
            await context.SaveChangesAsync();

            return this.Ok(producer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProducer(Guid id, [FromBody] ProducerInsertDto producerDto)
    {
        try
        {
            if (producerDto.Name.Length < 1 && producerDto.City.Length < 1)
            {
                return this.BadRequest($"Name and city must be non-empty string.");
            }
            var producer = await context.Producers.FindAsync(id);
            if (producer == null)
            {
                return this.NotFound($"Producer with ID {id} not found.");
            }
            var country = await context.Countries.FindAsync(producerDto.Country);
            if (country == null)
            {
                return this.NotFound($"Country with ID {producerDto.Country} not found.");
            }
            if (producerDto.IsEditFor != null)
            {
                var isEditFor = await context.Producers.FindAsync(producerDto.IsEditFor);
                if (isEditFor == null)
                {
                    return this.NotFound($"Producer with ID {producerDto.Country} not found.");
                }
            }

            producer.Name = producerDto.Name;
            producer.City = producerDto.City;
            producer.Description = producerDto.Description;
            producer.CountryId = producerDto.Country;
            producer.IsEditForId = producerDto.IsEditFor;

            context.Producers.Update(producer);
            await context.SaveChangesAsync();

            return this.Ok(producer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }
}
