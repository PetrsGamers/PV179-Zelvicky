namespace Cap.Enjoyer.WebMVC.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

public class CapCreateViewModel
{
    public string? TextOnCap { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IFormFile? CapPicture { get; set; }

    public List<Guid>? TextColorsIds { get; set; } = [];

    public List<Guid>? BgColorsIds { get; set; } = [];

    public List<Guid>? BottlesIds { get; set; }

    public required List<SelectListItem> BottlesOptions { get; set; }
    public required List<SelectListItem> ColorsOptions { get; set; }
}


