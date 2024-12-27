namespace Cap.Enjoyer.WebMVC.Controllers;
using Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class CapController(ICapService capService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var caps = await capService.GetAllCapsFilteredAsync();
        var viewModel = caps.Select(c => new CapViewModel
        {
            Id = c.Id,
            TextOnCap = c.TextOnCap,
            Description = c.Description,
            CapPicture = c.CapPicture,
            TextColors = c.TextColors,
            BgColors = c.BgColors,
            Bottles = c.Bottles,
            IsEditFor = c.IsEditFor
        });
        return View(viewModel);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var cap = await capService.GetCapByIdAsync(id);
        var viewModel = new CapViewModel
        {
            Id = cap.Id,
            TextOnCap = cap.TextOnCap,
            Description = cap.Description,
            CapPicture = cap.CapPicture,
            TextColors = cap.TextColors,
            BgColors = cap.BgColors,
            Bottles = cap.Bottles,
            IsEditFor = cap.IsEditFor
        };
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CapCreateViewModel viewModel, IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var capDto = new CapInsertDto
        {
            TextOnCap = viewModel.TextOnCap,
            Description = viewModel.Description,
            CapPicture = viewModel.CapPicture,
            TextColors = viewModel.TextColors,
            BgColors = viewModel.BgColors,
            Bottles = viewModel.Bottles,
            IsEditFor = viewModel.IsEditFor
        };

        var createdCap = await capService.CreateCapAsync(capDto);
        if (image != null)
        {
            await capService.UploadImageForCapAsync(createdCap.Id, image);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var cap = await capService.GetCapByIdAsync(id);
        var viewModel = new CapViewModel
        {
            Id = cap.Id,
            TextOnCap = cap.TextOnCap,
            Description = cap.Description,
            CapPicture = cap.CapPicture,
            TextColors = cap.TextColors,
            BgColors = cap.BgColors,
            Bottles = cap.Bottles,
            IsEditFor = cap.IsEditFor
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CapViewModel viewModel, IFormFile image)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var capDto = new CapInsertDto
        {
            TextOnCap = viewModel.TextOnCap,
            Description = viewModel.Description,
            CapPicture = viewModel.CapPicture,
            TextColors = viewModel.TextColors,
            BgColors = viewModel.BgColors,
            Bottles = viewModel.Bottles,
            IsEditFor = viewModel.IsEditFor
        };

        var updatedCap = await capService.UpdateCapAsync(viewModel.Id, capDto);
        if (image != null)
        {
            await capService.UploadImageForCapAsync(updatedCap.Id, image);
        }

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
