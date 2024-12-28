namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Entities;
using DTOs;
using Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ProducerService(CapEnjoyerDbContext context) : IProducerService
{
    public async Task<IEnumerable<ProducerDto>> GetAllProducersAsync()
    {
        var producers = await context.Producers
            .ToListAsync();

        return producers.Adapt<IEnumerable<ProducerDto>>();
    }

    public async Task<ProducerDto> GetProducerByIdAsync(Guid id)
    {
        var producer = await context.Producers
            .FirstOrDefaultAsync(p => p.Id == id) ?? throw new ArgumentException($"Producer with ID {id} not found.");

        return producer.Adapt<ProducerDto>();
    }


    public async Task DeleteProducerAsync(Guid id)
    {
        var producer = await context.Producers.FindAsync(id) ??
                       throw new ArgumentException($"Producer with ID {id} not found.");

        context.Producers.Remove(producer);
        await context.SaveChangesAsync();
    }

    public async Task<ProducerDto> CreateProducerAsync(ProducerInsertDto producerDto)
    {
        if (string.IsNullOrEmpty(producerDto.Name) || string.IsNullOrEmpty(producerDto.City))
        {
            throw new ArgumentException("Name and city must be non-empty string.");
        }

        var country = await context.Countries.FindAsync(producerDto.Country) ??
                      throw new ArgumentException($"Country with ID {producerDto.Country} not found.");

        if (producerDto.IsEditFor != null)
        {
            var isEditFor = await context.Producers.FindAsync(producerDto.IsEditFor) ??
                            throw new ArgumentException($"Producer with ID {producerDto.IsEditFor} not found.");
        }

        var producer = new Producer
        {
            Id = Guid.NewGuid(),
            Name = producerDto.Name,
            City = producerDto.City,
            Description = producerDto.Description,
            CountryId = producerDto.Country,
            Country = country,
            IsEditForId = producerDto.IsEditFor
        };

        await context.Producers.AddAsync(producer);
        await context.SaveChangesAsync();

        return producer.Adapt<ProducerDto>();
    }

    public async Task<ProducerDto> UpdateProducerAsync(Guid id, ProducerInsertDto producerDto)
    {
        if (producerDto.Name.Length < 1 && producerDto.City.Length < 1)
        {
            throw new ArgumentException("Name and city must be non-empty string.");
        }

        var oldProducer = await context.Producers.FindAsync(id) ??
                          throw new ArgumentException($"Producer with ID {producerDto} not found.");

        var country = await context.Countries.FindAsync(producerDto.Country) ??
                      throw new ArgumentException($"Country with ID {producerDto.Country} not found.");

        if (producerDto.Name.Length < 1 && producerDto.City.Length < 1)
        {
            throw new ArgumentException("Name and city must be non-empty string.");
        }

        if (producerDto.IsEditFor != null)
        {
            var isEditFor = await context.Producers.FindAsync(producerDto.IsEditFor) ??
                            throw new ArgumentException($"Producer with ID {producerDto.IsEditFor} not found.");
        }

        oldProducer.Name = producerDto.Name;
        oldProducer.City = producerDto.City;
        oldProducer.Description = producerDto.Description;
        oldProducer.CountryId = producerDto.Country;
        oldProducer.IsEditForId = producerDto.IsEditFor;

        await context.SaveChangesAsync();
        return oldProducer.Adapt<ProducerDto>();
    }

    public async Task<List<SelectListItem>> GetProducerOptionsAsync()
    {
        var producers = await context.Producers.Where(p => p.IsEditForId == null).ToListAsync();
        return producers.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
    }
}
