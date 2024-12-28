namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

public class CapController(ICapService capService, IBottleService bottleService, IColorService colorService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var caps = await capService.GetAllCapsFilteredAsync();
        var viewModel = caps.Select(c => new CapListViewModel
        {
            Id = c.Id,
            TextOnCap = c.TextOnCap,
            Description = c.Description,
            CapPicture = null,
            TextColorsIds = c.TextColors,
            BgColorsIds = c.BgColors,
            BottlesIds = c.Bottles
        });
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var cap = await capService.GetCapByIdAsync(id);

        var textColors = await colorService.GetColorsByIdsAsync(cap.TextColors);
        var bgColors = await colorService.GetColorsByIdsAsync(cap.BgColors);
        var bottles = await bottleService.GetBottlesByIdsAsync(cap.Bottles);

        var viewModel = new CapDetailViewModel
        {
            Id = cap.Id,
            TextOnCap = cap.TextOnCap,
            Description = cap.Description,
            CapPicture = cap.CapPicture,
            TextColors = textColors,
            BgColors = bgColors,
            Bottles = bottles,
            IsEditForId = cap.IsEditForId
        };

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
            var viewModel = new CapCreateViewModel
            {
                TextOnCap = model.TextOnCap,
                Description = model.Description,
                CapPicture = model.CapPicture,
                TextColorsIds = model.TextColorsIds,
                BgColorsIds = model.BgColorsIds,
                BottlesIds = model.BottlesIds,
                BottlesOptions = await bottleService.GetBottleOptionsAsync(),
                ColorsOptions = await colorService.GetColorOptionsAsync()
            };
            return View(viewModel);
        }

        var capDto = new CapInsertDto
        {
            TextOnCap = model.TextOnCap,
            Description = model.Description,
            TextColors = model.TextColorsIds,
            BgColors = model.BgColorsIds,
            Bottles = model.BottlesIds ?? []
        };

        await capService.CreateCapAsync(capDto);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var cap = await capService.GetCapByIdAsync(id);

        var model = new CapCreateViewModel
        {
            TextOnCap = cap.TextOnCap,
            Description = cap.Description,
            TextColorsIds = cap.TextColors,
            BgColorsIds = cap.BgColors,
            BottlesIds = cap.Bottles,
            BottlesOptions = await bottleService.GetBottleOptionsAsync(),
            ColorsOptions = await colorService.GetColorOptionsAsync()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, CapCreateReturnModel model)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new CapCreateViewModel
            {
                TextOnCap = model.TextOnCap,
                Description = model.Description,
                CapPicture = model.CapPicture,
                TextColorsIds = model.TextColorsIds,
                BgColorsIds = model.BgColorsIds,
                BottlesIds = model.BottlesIds,
                BottlesOptions = await bottleService.GetBottleOptionsAsync(),
                ColorsOptions = await colorService.GetColorOptionsAsync()
            };
            return View(viewModel);
        }

        var capDto = new CapInsertDto
        {
            TextOnCap = model.TextOnCap,
            Description = model.Description,
            CapPictureFile = model.CapPicture,
            TextColors = model.TextColorsIds,
            BgColors = model.BgColorsIds,
            Bottles = model.BottlesIds ?? [],
            IsEditFor = id
        };

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
