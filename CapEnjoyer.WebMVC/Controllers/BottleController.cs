using Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cap.Enjoyer.WebMVC.Controllers;

public class BottleController(IBottleService bottleService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var bottles = await bottleService.GetAllBottles();
        var viewModel = bottles.Select(b => new BottleViewModel
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description,
            Voltage = b.Voltage,
            BottlePicture = b.BottlePicture,
            DrinkType = b.DrinkType,
            Producer = b.Producer,
            Caps = b.Caps,
            IsEditFor = b.IsEditFor
        });
        return View(viewModel);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var bottle = await bottleService.GetBottleById(id);
        var viewModel = new BottleViewModel
        {
            Id = bottle.Id,
            Name = bottle.Name,
            Description = bottle.Description,
            Voltage = bottle.Voltage,
            BottlePicture = bottle.BottlePicture,
            DrinkType = bottle.DrinkType,
            Producer = bottle.Producer,
            Caps = bottle.Caps,
            IsEditFor = bottle.IsEditFor
        };
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create() =>
        View();

    [HttpPost]
    public async Task<IActionResult> Create(BottleCreateViewModel viewModel, IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var bottleDto = new BottleDto
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            Voltage = viewModel.Voltage,
            DrinkType = viewModel.DrinkType,
            Producer = viewModel.Producer,
            Caps = viewModel.Caps,
            IsEditFor = viewModel.IsEditFor
        };

        var createdBottle = await bottleService.CreateBottle(bottleDto);
        if (image != null)
        {
            await bottleService.UploadImageForBottleAsync(createdBottle.Id, image);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var bottle = await bottleService.GetBottleById(id);
        var viewModel = new BottleViewModel
        {
            Id = bottle.Id,
            Name = bottle.Name,
            Description = bottle.Description,
            Voltage = bottle.Voltage,
            BottlePicture = bottle.BottlePicture,
            DrinkType = bottle.DrinkType,
            Producer = bottle.Producer,
            Caps = bottle.Caps,
            IsEditFor = bottle.IsEditFor
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BottleViewModel viewModel, IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var bottleDto = new BottleDto
        {
            Id = viewModel.Id,
            Name = viewModel.Name,
            Description = viewModel.Description,
            Voltage = viewModel.Voltage,
            DrinkType = viewModel.DrinkType,
            Producer = viewModel.Producer,
            Caps = viewModel.Caps,
            IsEditFor = viewModel.IsEditFor
        };

        var updatedBottle = await bottleService.UpdateBottle(viewModel.Id, bottleDto);
        if (image != null)
        {
            await bottleService.UploadImageForBottleAsync(updatedBottle.Id, image);
        }

        return RedirectToAction(nameof(Index));
    }
}
