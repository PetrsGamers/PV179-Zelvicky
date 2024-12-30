namespace CapEnjoyer.BL.Services;

using DAL;
using DTOs;
using Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class CountryService(CapEnjoyerDbContext context) : ICountryService
{
    public async Task<List<CountryDto>> GetCountriesAsync()
    {
        var countries = await context.Countries
            .OrderBy(c => c.Name)
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

    public async Task<List<SelectListItem>> GetCountryOptionsAsync()
    {
        var countries = await context.Countries
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();

        return countries;
    }
}
