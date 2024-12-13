namespace Cap.Enjoyer.WebMVC.Controllers;

using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{

    public string Index()
    {
        return "This is my default action...";
    }

    public string Welcome()
    {
        return "This is the Welcome action method...";
    }
}

