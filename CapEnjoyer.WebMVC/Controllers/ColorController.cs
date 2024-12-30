namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;

public class ColorController(IColorService colorService, IMemoryCache memoryCache) : Controller
{
    public async Task<IActionResult> Index()
    {
        const string cacheKey = "AllColors";
        var cachedColors = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var colorDtos = await colorService.GetColorsAsync();
            return colorDtos.Select(c => c.Adapt<ColorListViewModel>());
        });

        return View(cachedColors);
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
        memoryCache.Remove("AllColors");
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
        memoryCache.Remove("AllColors");
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

        memoryCache.Remove("AllColors");
        return RedirectToAction(nameof(Index));
    }
}
