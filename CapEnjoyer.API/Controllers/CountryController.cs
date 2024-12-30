namespace CapEnjoyer.API.Controllers;

using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CountryController(ICountryService countryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCountries()
    {
        var countries = await countryService.GetCountriesAsync();
        return Ok(countries);
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
