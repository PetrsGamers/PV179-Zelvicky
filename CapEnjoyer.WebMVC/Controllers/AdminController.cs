namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.Services.Interfaces;
using CapEnjoyer.DAL.Constants;
using CapEnjoyer.DAL.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

public class AdminController(
    UserManager<LocalIdentityUser> userManager,
    IUserService userService,
    ICouponService couponService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> ResetUserPassword()
    {
        var user = await userManager.GetUserAsync(User);
        var userDB = await userService.GetUserById(user.UserId);
        if (userDB == null || userDB.Role != Role.Admin)
        {
            return this.RedirectToAction("Index", "Home");
        }

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ResetUserPassword(ResetUserPasswordReturnModel model)
    {
        if (!ModelState.IsValid)
        {
            return this.View(model);
        }

        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            this.ModelState.AddModelError(string.Empty, "You are not signed in.");
            return this.View(model);
        }

        var userDB = await userService.GetUserById(signInUser.UserId);
        if (userDB.Role != Role.Admin)
        {
            this.ModelState.AddModelError(string.Empty, "You are not an admin.");
            return this.View(model);
        }

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            this.ModelState.AddModelError(string.Empty, "User not found.");
            return this.View(model);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
        if (result.Succeeded)
        {
            return this.RedirectToAction(nameof(this.ResetUserPassword),
                nameof(AdminController).Replace("Controller", ""));
        }

        return this.View(model);

    }

    public async Task<IActionResult> CouponList()
    {
        var signInUser = await userManager.GetUserAsync(User);
        if (signInUser == null)
        {
            return RedirectToAction("Login", "Account");
        }


        var coupons = await couponService.GetCouponsAsync();

        var viewModel = coupons.Select(c => c.Adapt<CouponViewModel>());
        return View(viewModel);
    }
}
