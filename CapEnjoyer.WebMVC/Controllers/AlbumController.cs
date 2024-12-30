namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using CapEnjoyer.DAL.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;

public class AlbumController(
    IAlbumService albumService,
    ICapService capService,
    IUserService userService,
    UserManager<LocalIdentityUser> userManager,
    IMemoryCache memoryCache) : Controller
{
    public async Task<IActionResult> Index()
    {
        var albums = await albumService.GetAllAlbums();
        var users = await userService.GetAllUsers();
        var signInUser = await userManager.GetUserAsync(User);
        var viewModel = new AlbumListViewModel
        {
            Albums = albums.Adapt<List<AlbumViewModel>>(),
            LoggedUser = signInUser?.UserId
        };
        viewModel.Albums.ForEach(vm => vm.Username = users.FirstOrDefault(u => u.Id == vm.UserId)?.Username);
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var caps = await capService.GetAllCapsFilteredAsync();
        var viewModel = new AlbumCreateViewModel
        {
            Caps = caps.Where(c => c.IsEditForId == null).Select(c => new CapDto
            {
                Id = c.Id,
                TextOnCap = c.TextOnCap
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AlbumCreateReturnModel model)
    {
        var signInUser = await userManager.GetUserAsync(User);
        model.UserId = signInUser.UserId;
        if (!ModelState.IsValid)
        {
            var caps = await capService.GetAllCapsFilteredAsync();
            var viewmodel = model.Adapt<AlbumCreateViewModel>();
            viewmodel.Caps = caps.Where(c => c.IsEditForId == null).Select(c => new CapDto
            {
                Id = c.Id,
                TextOnCap = c.TextOnCap
            }).ToList();

            return View(viewmodel);
        }

        var album = model.Adapt<AlbumInsertDto>();

        await albumService.CreateAlbum(album);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var album = await albumService.GetAlbumById(id);
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (album.UserId != signInUser.UserId)
        {
            return RedirectToAction("Index");
        }

        var caps = await capService.GetAllCapsFilteredAsync();
        var viewmodel = album.Adapt<AlbumCreateViewModel>();
        viewmodel.Caps = caps.Where(c => c.IsEditForId == null)
            .Select(c => new CapDto { Id = c.Id, TextOnCap = c.TextOnCap }).ToList();
        viewmodel.SelectedCapIds = album.Caps;
        return View(viewmodel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AlbumCreateReturnModel model)
    {
        var signInUser = await userManager.GetUserAsync(User);
        model.UserId = signInUser.UserId;
        if (!ModelState.IsValid)
        {
            var caps = await capService.GetAllCapsFilteredAsync();
            var viewmodel = model.Adapt<AlbumCreateViewModel>();
            viewmodel.Caps = caps.Select(c => new CapDto { Id = c.Id, TextOnCap = c.TextOnCap }).ToList();
            return View(viewmodel);
        }

        var album = model.Adapt<AlbumInsertDto>();
        await albumService.UpdateAlbum(model.Id, album);

        memoryCache.Remove($"AlbumDetails_{model.Id}");
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var cacheKey = $"AlbumDetails_{id}";
        var cachedViewModel = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var album = await albumService.GetAlbumById(id);
            var user = await userService.GetUserById(album.UserId);
            var caps = await capService.GetAllCapsByAlbumIdAsync(id);
            var viewModel = album.Adapt<AlbumDetailViewModel>();
            viewModel.Caps = caps.ToList();
            viewModel.Username = user.Username;
            return viewModel;
        });

        return View(cachedViewModel);
    }

    public async Task<IActionResult> DeleteCap(Guid albumId, Guid capId)
    {
        var album = await albumService.GetAlbumById(albumId);
        if (album.Caps.Contains(capId))
        {
            await albumService.RemoveCapFromAlbum(albumId, capId);
        }

        return RedirectToAction("Details", new { id = albumId });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        await albumService.DeleteAlbum(id);
        memoryCache.Remove($"AlbumDetails_{id}");
        return RedirectToAction("Index");
    }
}
