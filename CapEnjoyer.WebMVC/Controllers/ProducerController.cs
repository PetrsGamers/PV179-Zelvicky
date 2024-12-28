namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

public class ProducerController(IProducerServiceAsync producerService, ICountryService countryService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var producers = await producerService.GetAllProducersAsync();
        var viewModel = producers.Select(p => new ProducerViewModel
        {
            Id = p.Id,
            Name = p.Name,
            City = p.City,
            Description = p.Description,
            Country = p.CountryId,
            IsEditFor = p.IsEditForId
        });
        return View(viewModel);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var producer = await producerService.GetProducerByIdAsync(id);
        var viewModel = new ProducerViewModel
        {
            Id = producer.Id,
            Name = producer.Name,
            City = producer.City,
            Description = producer.Description,
            Country = producer.CountryId
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var countries = await countryService.GetCountriesAsync();
        var defaultCountryId = countries.FirstOrDefault(c => c.Name == "Czech Republic")?.Id ?? Guid.Empty;

        var viewModel = new ProducerCreateViewModel
        {
            CountryId = defaultCountryId,
            Countries = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProducerCreateReturnModel returnModel)
    {
        if (!ModelState.IsValid)
        {
            var countries = await countryService.GetCountriesAsync();
            var viewModel = new ProducerCreateViewModel
            {
                Name = returnModel.Name,
                City = returnModel.City,
                Description = returnModel.Description,
                CountryId = returnModel.CountryId,
                Countries = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList()
            };

            return View(viewModel);
        }

        var producerDto = new ProducerInsertDto
        {
            Name = returnModel.Name,
            City = returnModel.City,
            Description = returnModel.Description,
            Country = returnModel.CountryId
        };

        await producerService.CreateProducerAsync(producerDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var producer = await producerService.GetProducerByIdAsync(id);
        var countries = await countryService.GetCountriesAsync();

        var viewModel = new ProducerCreateViewModel
        {
            Name = producer.Name,
            City = producer.City,
            Description = producer.Description,
            CountryId = producer.CountryId,
            Countries = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, ProducerCreateReturnModel returnModel)
    {
        if (!ModelState.IsValid)
        {
            var countries = await countryService.GetCountriesAsync();
            var viewModel = new ProducerCreateViewModel
            {
                Name = returnModel.Name,
                City = returnModel.City,
                Description = returnModel.Description,
                CountryId = returnModel.CountryId,
                Countries = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList()
            };
            return View(viewModel);
        }

        var producerDto = new ProducerInsertDto
        {
            Name = returnModel.Name,
            City = returnModel.City,
            Description = returnModel.Description,
            Country = returnModel.CountryId
        };

        await producerService.UpdateProducerAsync(id, producerDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await producerService.DeleteProducerAsync(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
