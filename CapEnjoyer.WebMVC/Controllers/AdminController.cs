namespace Cap.Enjoyer.WebMVC.Controllers;

using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public string Index() => "This is my default action...";

    public string Welcome() => "This is the Welcome action method...";
}
