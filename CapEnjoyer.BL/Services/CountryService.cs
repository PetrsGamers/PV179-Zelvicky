namespace CapEnjoyer.BL.Services;

using DAL;
using DTOs;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class CountryService(CapEnjoyerDbContext context) : ICountryService
{
    // implement ICountryService
    public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
    {
        var countries = await context.Countries
            .Select(c => new CountryDto { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return countries;
    }

    public async Task<CountryDto?> GetCountryByIdAsync(Guid id)
    {
        var country = await context.Countries
            .Where(c => c.Id == id)
            .Select(c => new CountryDto { Id = c.Id, Name = c.Name })
            .FirstOrDefaultAsync();

        return country;
    }
}
