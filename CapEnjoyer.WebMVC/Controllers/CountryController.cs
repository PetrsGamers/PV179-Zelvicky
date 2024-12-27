namespace Cap.Enjoyer.WebMVC.Controllers;
using Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class CountryController(ICountryService countryService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var countries = await countryService.GetCountriesAsync();
        var viewModel = countries.Select(c => new CountryViewModel
        {
            Id = c.Id,
            Name = c.Name
        });
        return View(viewModel);
    }
}
