namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.DTOs;
using CapEnjoyer.BL.Services.Interfaces;
using CapEnjoyer.DAL.Entities;
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

        var viewModel = coupons.Select(c => new CouponViewModel
        {
            Id = c.Id,
            Code = c.Code,
            IsUsed = c.IsUsed,
            ValidFrom = c.ValidFrom,
            ValidTo = c.ValidUntil,
            ActivateeName = c.ActivateeUsername
        });
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
    public async Task<IActionResult> ActivateConfirmed(CouponReturnModel model)
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var coupon = await couponService.ActivateCouponAsync(model.Code, signInUser.UserId);
        if (coupon == null)
        {
            Console.WriteLine("Coupon not found or already used");
            return RedirectToAction("Activate");
        }

        Console.WriteLine("Coupon activated");
        return RedirectToAction("Index");
    }
}
