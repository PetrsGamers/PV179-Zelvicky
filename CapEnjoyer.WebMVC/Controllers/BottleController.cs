namespace Cap.Enjoyer.WebMVC.Controllers;
using Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using CapEnjoyer.DAL.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class BottleController(IBottleService bottleService, IProducerService producerService, ICapService capService) : Controller
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
    public async Task<IActionResult> Create()
    {
        var model = new BottleCreateViewModel
        {
            ProducersOptions = await producerService.GetProducerOptionsAsync(),
            DrinkTypes = bottleService.GetDrinkTypeOptions(),
            CapsOptions = await capService.GetCapOptionsAsync()
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Create(BottleCreateReturnModel model)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new BottleCreateViewModel
            {
                Name = model.Name,
                Description = model.Description,
                Voltage = model.Voltage,
                BottlePicture = null, //TODO image input
                DrinkType = model.DrinkType,
                ProducerId = model.ProducerId,
                CapIds = model.CapIds,
                ProducersOptions = await producerService.GetProducerOptionsAsync(),
                DrinkTypes = bottleService.GetDrinkTypeOptions(),
                CapsOptions = await capService.GetCapOptionsAsync()
            };

            return View(viewModel);
        }

        var bottleDto = new BottleInsertDto
        {
            Name = model.Name,
            Description = model.Description,
            Voltage = model.Voltage,
            DrinkType = model.DrinkType,
            Producer = model.ProducerId,
            Caps = model.CapIds ?? [],
            BottlePicture = "placeholder"
        };

        await bottleService.CreateBottle(bottleDto);
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var bottle = await bottleService.GetBottleById(id);

        var viewModel = new BottleCreateViewModel
        {
            Name = bottle.Name,
            Description = bottle.Description,
            Voltage = bottle.Voltage,
            BottlePicture = null,
            DrinkType = bottle.DrinkType,
            ProducerId = bottle.Producer,
            CapIds = bottle.Caps,
            ProducersOptions = await producerService.GetProducerOptionsAsync(),
            DrinkTypes = bottleService.GetDrinkTypeOptions(),
            CapsOptions = await capService.GetCapOptionsAsync()
        };

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, BottleCreateReturnModel model)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new BottleCreateViewModel
            {
                Name = model.Name,
                Description = model.Description,
                Voltage = model.Voltage,
                BottlePicture = null, //TODO image input
                DrinkType = model.DrinkType,
                ProducerId = model.ProducerId,
                CapIds = model.CapIds,
                ProducersOptions = await producerService.GetProducerOptionsAsync(),
                DrinkTypes = bottleService.GetDrinkTypeOptions(),
                CapsOptions = await capService.GetCapOptionsAsync()
            };

            return View(viewModel);
        }

        var bottleDto = new BottleInsertDto()
        {
            Name = model.Name,
            Description = model.Description,
            Voltage = model.Voltage,
            DrinkType = model.DrinkType,
            Producer = model.ProducerId,
            Caps = model.CapIds ?? []
        };

        await bottleService.UpdateBottle(id, bottleDto);

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await bottleService.DeleteBottle(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
