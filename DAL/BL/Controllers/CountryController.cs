using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAL.BL.Controllers;

using DTOs;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class CountryController(ApplicationContext context, ILogger<CountryController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        try
        {
            var countries = await context.Countries
                .Select(c => new CountryDto { Id = c.Id, Name = c.Name })
                .ToListAsync();

            return this.Ok(countries);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCountryById(Guid id)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }
}
