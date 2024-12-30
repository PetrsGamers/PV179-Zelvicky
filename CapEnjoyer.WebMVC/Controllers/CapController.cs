namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Models;

public class CapController(ICapService capService, IBottleService bottleService, IColorService colorService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var caps = await capService.GetAllCapsFilteredAsync();
        var viewModel = caps.Where(c => c.IsEditForId is null).Select(c => c.Adapt<CapListViewModel>());
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var cap = await capService.FindCapWithDetailsByIdAsync(id);

        if (cap is null)
        {
            return NotFound();
        }

        var viewModel = cap.Adapt<CapDetailViewModel>();
        return View(viewModel);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CapCreateViewModel
        {
            ColorsOptions = await colorService.GetColorOptionsAsync(),
            BottlesOptions = await bottleService.GetBottleOptionsAsync()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CapCreateReturnModel model)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = model.Adapt<CapCreateViewModel>();
            viewModel.BottlesOptions = await bottleService.GetBottleOptionsAsync();
            viewModel.ColorsOptions = await colorService.GetColorOptionsAsync();

            return View(viewModel);
        }

        var capDto = model.Adapt<CapInsertDto>();
        await capService.CreateCapAsync(capDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var cap = await capService.GetCapByIdAsync(id);

        var viewModel = cap.Adapt<CapCreateViewModel>();
        viewModel.BottlesOptions = await bottleService.GetBottleOptionsAsync();
        viewModel.ColorsOptions = await colorService.GetColorOptionsAsync();
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, CapCreateReturnModel model)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = model.Adapt<CapCreateViewModel>();
            viewModel.BottlesOptions = await bottleService.GetBottleOptionsAsync();
            viewModel.ColorsOptions = await colorService.GetColorOptionsAsync();

            return View(viewModel);
        }

        var capDto = model.Adapt<CapInsertDto>();
        capDto.IsEditForId = id;
        await capService.CreateCapAsync(capDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await capService.DeleteCapAsync(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
