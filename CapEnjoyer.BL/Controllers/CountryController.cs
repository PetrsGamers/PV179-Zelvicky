namespace CapEnjoyer.BL.Controllers;

using DAL;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CountryController(CapEnjoyerDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        var countries = await context.Countries
            .Select(c => new CountryDto { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return this.Ok(countries);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCountryById(Guid id)
    {
        var country = await context.Countries
            .Where(c => c.Id == id)
            .Select(c => new CountryDto { Id = c.Id, Name = c.Name })
            .FirstOrDefaultAsync();

        if (country == null)
        {
            return this.NotFound($"Country with ID {id} not found.");
        }

        return this.Ok(country);
    }
}
