using Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cap.Enjoyer.WebMVC.Controllers;

public class ColorController(IColorService colorService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var colors = await colorService.GetColorsAsync();
        var viewModel = colors.Select(c => new ColorViewModel
        {
            Id = c.Id,
            Name = c.Name,
            HexValue = c.HexValue
        });
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ColorCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var colorDto = new ColorDto
        {
            Name = viewModel.Name,
            HexValue = viewModel.HexValue
        };

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

        var viewModel = new ColorViewModel
        {
            Id = color.Id,
            Name = color.Name,
            HexValue = color.HexValue
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ColorViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var colorDto = new ColorDto
        {
            Id = viewModel.Id,
            Name = viewModel.Name,
            HexValue = viewModel.HexValue
        };

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
