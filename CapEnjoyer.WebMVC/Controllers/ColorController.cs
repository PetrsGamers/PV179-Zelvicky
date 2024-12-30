namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Models;

public class ColorController(IColorService colorService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var colors = await colorService.GetColorsAsync();
        var viewModel = colors.Select(c => c.Adapt<ColorListViewModel>());
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ColorCreateReturnModel returnModel)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = returnModel.Adapt<ColorCreateViewModel>();
            return View(viewModel);
        }

        var colorDto = returnModel.Adapt<ColorInsertDto>();

        await colorService.CreateColorAsync(colorDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var color = await colorService.GetColorByIdAsync(id);
        if (color is null)
        {
            return NotFound();
        }

        var viewModel = color.Adapt<ColorCreateViewModel>();
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, ColorCreateReturnModel returnModel)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = returnModel.Adapt<ColorCreateViewModel>();
            return View(viewModel);
        }

        var colorDto = returnModel.Adapt<ColorInsertDto>();

        await colorService.UpdateColorAsync(id, colorDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await colorService.DeleteColorAsync(id);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
