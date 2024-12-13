using Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cap.Enjoyer.WebMVC.Controllers;

public class ProducerController(IProducerService producerService) : Controller
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
            Country = p.Country,
            IsEditFor = p.IsEditFor
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
            Country = producer.Country,
            IsEditFor = producer.IsEditFor
        };
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ProducerCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var producerDto = new ProducerInsertDto
        {
            Name = viewModel.Name,
            City = viewModel.City,
            Description = viewModel.Description,
            Country = viewModel.Country,
            IsEditFor = viewModel.IsEditFor
        };

        await producerService.CreateProducerAsync(producerDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var producer = await producerService.GetProducerByIdAsync(id);
        var viewModel = new ProducerViewModel
        {
            Id = producer.Id,
            Name = producer.Name,
            City = producer.City,
            Description = producer.Description,
            Country = producer.Country,
            IsEditFor = producer.IsEditFor
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProducerViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var producerDto = new ProducerInsertDto
        {
            Name = viewModel.Name,
            City = viewModel.City,
            Description = viewModel.Description,
            Country = viewModel.Country,
            IsEditFor = viewModel.IsEditFor
        };

        await producerService.UpdateProducerAsync(viewModel.Id, producerDto);
        return RedirectToAction(nameof(Index));
    }
}
