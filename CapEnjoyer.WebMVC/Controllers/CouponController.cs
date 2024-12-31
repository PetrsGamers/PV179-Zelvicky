namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using CapEnjoyer.DAL.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

public class CouponController(
    ICouponService couponService,
    IUserService userService,
    UserManager<LocalIdentityUser> userManager
) : Controller
{
    public async Task<IActionResult> Index()
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var coupons = await couponService.GetCouponsByBuyerIdAsync(signInUser.UserId);
        var viewModel = coupons.Select(c => c.Adapt<CouponViewModel>());
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Buy()
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> BuyConfirmed()
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var couponInsertDto = new CouponInsertDto { BuyerId = signInUser.UserId };
        await couponService.CreateNewCouponAsync(couponInsertDto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Activate()
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Activate(CouponReturnModel model)
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var coupon = await couponService.ActivateCouponAsync(model.Code, signInUser.UserId);
        if (coupon == null)
        {
            TempData["ErrorMessage"] = "Coupon not found, already used or you have an active coupon already";
            return RedirectToAction("Activate");
        }

        TempData["SuccessMessage"] = "Coupon activated";
        return RedirectToAction("Index");
    }
}
