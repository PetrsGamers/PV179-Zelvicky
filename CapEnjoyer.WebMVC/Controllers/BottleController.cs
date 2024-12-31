namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Models;

public class BottleController(
    IBottleService bottleService,
    IProducerService producerService,
    IImageService imageService,
    ICapService capService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var bottles = await bottleService.GetAllBottlesAsync();
        var viewModel = bottles.Where(b => b.IsEditForId is null).Select(b => b.Adapt<BottleListViewModel>());
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var bottle = await bottleService.FindBottleWithDetailsByIdAsync(id);

        if (bottle is null)
        {
            return NotFound();
        }

        var viewModel = bottle.Adapt<BottleDetailViewModel>();

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
            var viewModel = model.Adapt<BottleCreateViewModel>();
            viewModel.ProducersOptions = await producerService.GetProducerOptionsAsync();
            viewModel.DrinkTypes = bottleService.GetDrinkTypeOptions();
            viewModel.CapsOptions = await capService.GetCapOptionsAsync();

            return View(viewModel);
        }

        var bottleDto = model.Adapt<BottleInsertDto>();
        var bottle = await bottleService.CreateBottleAsync(bottleDto);
        if (model.BottlePictureFile is not null)
        {
            await imageService.UploadImageForBottleAsync(bottle.Id, model.BottlePictureFile);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var bottle = await bottleService.GetBottleByIdAsync(id);

        var viewModel = bottle.Adapt<BottleCreateViewModel>();
        viewModel.ProducersOptions = await producerService.GetProducerOptionsAsync();
        viewModel.DrinkTypes = bottleService.GetDrinkTypeOptions();
        viewModel.CapsOptions = await capService.GetCapOptionsAsync();

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, BottleCreateReturnModel model)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = model.Adapt<BottleCreateViewModel>();
            viewModel.ProducersOptions = await producerService.GetProducerOptionsAsync();
            viewModel.DrinkTypes = bottleService.GetDrinkTypeOptions();
            viewModel.CapsOptions = await capService.GetCapOptionsAsync();

            return View(viewModel);
        }

        var bottleDto = model.Adapt<BottleInsertDto>();
        bottleDto.IsEditForId = id;

        await bottleService.CreateBottleAsync(bottleDto);

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await bottleService.DeleteBottleAsync(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
