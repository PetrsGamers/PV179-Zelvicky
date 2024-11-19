namespace Cap.Enjoyer.WebMVC.Controllers;
using System.Diagnostics;
using Cap.Enjoyer.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    public IActionResult Index() => this.View();

    public IActionResult Privacy() => this.View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
}
