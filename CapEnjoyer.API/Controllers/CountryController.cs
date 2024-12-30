namespace CapEnjoyer.API.Controllers;

using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Route("api/[controller]")]
public class CountryController(ICountryService countryService, IMemoryCache memoryCache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        const string cacheKey = "AllCountries";
        var cachedCountries = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return await countryService.GetCountriesAsync();
        });
        return Ok(cachedCountries);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCountryById(Guid id)
    {
        var country = await countryService.GetCountryByIdAsync(id);
        if (country is null)
        {
            return NotFound($"Country with ID {id} not found.");
        }

        return Ok(country);
    }
}
