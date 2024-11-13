namespace CapEnjoyer.BL.Controllers;

using DAL;
using DAL.Entities;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProducerController(CapEnjoyerDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducers()
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


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProducerById(Guid id)
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


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProducer(Guid id)
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


    [HttpPost]
    public async Task<IActionResult> CreateProducer([FromBody] ProducerInsertDto producerDto)
    {
        if (producerDto.Name.Length < 1 && producerDto.City.Length < 1)
        {
            return this.BadRequest("Name and city must be non-empty string.");
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
                return this.NotFound($"Producer with ID {producerDto.IsEditFor} not found.");
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


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProducer(Guid id, [FromBody] ProducerInsertDto producerDto)
    {
        if (producerDto.Name.Length < 1 && producerDto.City.Length < 1)
        {
            return this.BadRequest("Name and city must be non-empty string.");
        }

        var oldProducer = await context.Producers.FindAsync(id);
        if (oldProducer == null)
        {
            return this.NotFound($"Producer with ID {id} not found.");
        }

        var country = await context.Countries.FindAsync(producerDto.Country);
        if (country == null)
        {
            return this.NotFound($"Country with ID {producerDto.Country} not found.");
        }

        if (producerDto.Name.Length < 1 && producerDto.City.Length < 1)
        {
            return this.BadRequest("Name and city must be non-empty string.");
        }

        if (producerDto.IsEditFor != null)
        {
            var isEditFor = await context.Producers.FindAsync(producerDto.IsEditFor);
            if (isEditFor == null)
            {
                return this.NotFound($"Producer with ID {producerDto.IsEditFor} not found.");
            }
        }

        var newProducer = new Producer
        {
            Id = Guid.NewGuid(),
            Name = producerDto.Name,
            City = producerDto.City,
            Description = producerDto.Description,
            CountryId = producerDto.Country,
            IsEditForId = oldProducer.Id
        };

        await context.Producers.AddAsync(newProducer);
        await context.SaveChangesAsync();

        return this.Ok(newProducer);
    }
}
