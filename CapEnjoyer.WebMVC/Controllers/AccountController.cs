namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.DAL.Constants;
using CapEnjoyer.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

public class AccountController(UserManager<LocalIdentityUser> userManager, SignInManager<LocalIdentityUser> signInManager) : Controller
{
    public IActionResult Register() => this.View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (this.ModelState.IsValid)
        {
            var user = new LocalIdentityUser { UserName = model.Email, Email = model.Email, User = new() { Id = new Guid(), Username = model.Email, Email = model.Email, Albums = [], Role = Role.User } };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
                return this.View(model);
            }

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                // return RedirectToAction("Login", "Account");
                return this.RedirectToAction(nameof(Login), nameof(AccountController).Replace("Controller", ""));
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return this.View(model);
    }

    public IActionResult Login() => this.View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (this.ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // return RedirectToAction("LoginSuccess", "Account");
                return this.RedirectToAction(nameof(LoginSuccess), nameof(AccountController).Replace("Controller", ""));
            }

            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return this.View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        // return RedirectToAction("Index", "Home");
        return this.RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""));
    }

    public IActionResult LoginSuccess() => this.View();
}

