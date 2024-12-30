namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

public class ProducerController(IProducerService producerService, ICountryService countryService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var producers = await producerService.GetAllProducersAsync();
        var viewModel = producers.Where(p => p.IsEditForId is null).Select(p => p.Adapt<ProducerListViewModel>());
        return View(viewModel);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var producer = await producerService.FindProducerWithDetailsByIdAsync(id);

        if (producer is null)
        {
            return NotFound();
        }

        var viewModel = producer.Adapt<ProducerDetailViewModel>();

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
            Countries = await countryService.GetCountryOptionsAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProducerCreateReturnModel returnModel)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = returnModel.Adapt<ProducerCreateViewModel>();
            viewModel.Countries = await countryService.GetCountryOptionsAsync();

            return View(viewModel);
        }

        var producerDto = returnModel.Adapt<ProducerInsertDto>();
        await producerService.CreateProducerAsync(producerDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var producer = await producerService.GetProducerByIdAsync(id);
        var viewModel = producer.Adapt<ProducerCreateViewModel>();
        viewModel.Countries = await countryService.GetCountryOptionsAsync();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, ProducerCreateReturnModel returnModel)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = returnModel.Adapt<ProducerCreateViewModel>();
            viewModel.Countries = await countryService.GetCountryOptionsAsync();

            return View(viewModel);
        }

        var producerDto = returnModel.Adapt<ProducerInsertDto>();
        producerDto.IsEditForId = id;
        await producerService.CreateProducerAsync(producerDto);

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
        catch (DbUpdateException)
        {
            //TODO hlaska
        }

        return RedirectToAction(nameof(Index));
    }
}
