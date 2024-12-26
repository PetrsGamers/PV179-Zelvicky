namespace CapEnjoyer.BL.Services;

using DAL;
using DTOs;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

public class CountryService(CapEnjoyerDbContext context) : ICountryService
{
    public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
    {
        var countries = await context.Countries
            .Select(c => c.Adapt<CountryDto>())
            .ToListAsync();

        return countries;
    }

    public async Task<CountryDto?> GetCountryByIdAsync(Guid id)
    {
        var country = await context.Countries
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        return country.Adapt<CountryDto>();
    }
}
