namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.DAL.Constants;
using CapEnjoyer.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

public class AccountController(
    UserManager<LocalIdentityUser> userManager,
    SignInManager<LocalIdentityUser> signInManager) : Controller
{
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new LocalIdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                User = new User
                {
                    Id = new Guid(),
                    Username = model.Email,
                    Email = model.Email,
                    Albums = [],
                    Role = Role.User
                }
            };
            var result = await userManager.CreateAsync(user, model.Password);
            //this is a temporary solution, will be resolved in M4
            if (model.Email.Contains("admin@admin.cz"))
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                return RedirectToAction(nameof(Login), nameof(AccountController).Replace("Controller", ""));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(LoginSuccess),
                    nameof(AccountController).Replace("Controller", ""));
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""));
    }

    public IActionResult LoginSuccess() => View();
}
