namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

public class BottleController(
    IBottleService bottleService,
    IProducerService producerService,
    ICapService capService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var bottles = await bottleService.GetAllBottles();
        var viewModel = bottles.Select(b => new BottleListViewModel
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description,
            Voltage = b.Voltage,
            BottlePicture = b.BottlePicture,
            DrinkType = b.DrinkType,
            Producer = b.ProducerId,
            Caps = b.Caps ?? [],
            IsEditFor = b.IsEditForId
        });
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var bottle = await bottleService.GetBottleById(id);

        var producer = await producerService.GetProducerByIdAsync(bottle.ProducerId);

        var capIds = bottle.Caps ?? [];
        List<CapDetail> caps = [];

        foreach (var capId in capIds)
        {
            var cap = await capService.GetCapByIdAsync(capId);
            caps.Add(new CapDetail { Id = cap.Id, Name = cap.TextOnCap, Description = cap.Description });
        }

        var viewModel = new BottleDetailViewModel
        {
            Id = bottle.Id,
            Name = bottle.Name,
            Description = bottle.Description,
            Voltage = bottle.Voltage,
            BottlePicture = bottle.BottlePicture,
            DrinkType = bottle.DrinkType,
            Producer = bottle.ProducerId,
            ProducerName = producer.Name,
            CapDetails = caps,
            IsEditFor = bottle.IsEditForId
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
            ProducerId = model.ProducerId,
            Caps = model.CapIds ?? [],
            BottlePictureFile = model.BottlePicture
        };

        await bottleService.CreateBottle(bottleDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var bottle = await bottleService.GetBottleById(id);

        var model = new BottleCreateViewModel
        {
            Name = bottle.Name,
            Description = bottle.Description,
            Voltage = bottle.Voltage,
            DrinkType = bottle.DrinkType,
            ProducerId = bottle.ProducerId,
            CapIds = bottle.Caps,
            ProducersOptions = await producerService.GetProducerOptionsAsync(),
            DrinkTypes = bottleService.GetDrinkTypeOptions(),
            CapsOptions = await capService.GetCapOptionsAsync()
        };

        return View(model);
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
                BottlePicture = model.BottlePicture,
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
            ProducerId = model.ProducerId,
            Caps = model.CapIds ?? [],
            IsEditForId = id
        };

        await bottleService.CreateBottle(bottleDto);

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
