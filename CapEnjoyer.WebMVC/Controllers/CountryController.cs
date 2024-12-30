namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;

public class CountryController(ICountryService countryService, IMemoryCache memoryCache) : Controller
{
    public async Task<IActionResult> Index()
    {
        const string cacheKey = "AllCountries";
        var cachedCountries = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);

            var countryDtos = await countryService.GetCountriesAsync();
            return countryDtos.Select(c => new CountryViewModel { Id = c.Id, Name = c.Name })
                .ToList();
        });
        return View(cachedCountries);
    }
}
