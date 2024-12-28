namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

public class CountryController(ICountryService countryService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var countries = await countryService.GetCountriesAsync();
        var viewModel = countries.Select(c => new CountryViewModel { Id = c.Id, Name = c.Name });
        return View(viewModel);
    }
}
