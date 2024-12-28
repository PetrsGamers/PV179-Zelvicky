namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

public class ColorController(IColorService colorService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var colors = await colorService.GetColorsAsync();
        var viewModel = colors.Select(c => new ColorViewModel { Id = c.Id, Name = c.Name, HexValue = c.HexCode });
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ColorCreateReturnModel returnModel)
    {
        if (!ModelState.IsValid)
        {
            return View(returnModel);
        }

        var colorDto = new ColorDto { Name = returnModel.Name, HexCode = returnModel.HexValue };

        await colorService.CreateColorAsync(colorDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var color = await colorService.GetColorByIdAsync(id);
        if (color == null)
        {
            return NotFound();
        }

        var viewModel = new ColorViewModel { Id = color.Id, Name = color.Name, HexValue = color.HexCode };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ColorViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var colorDto = new ColorDto { Id = viewModel.Id, Name = viewModel.Name, HexCode = viewModel.HexValue };

        await colorService.UpdateColorAsync(viewModel.Id, colorDto);
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
