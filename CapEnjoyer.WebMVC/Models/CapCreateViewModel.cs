namespace Cap.Enjoyer.WebMVC.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

public class CapCreateViewModel
{
    public string? TextOnCap { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IFormFile? CapPictureFile { get; set; }

    public List<Guid>? TextColorIds { get; set; } = [];

    public List<Guid>? BgColorIds { get; set; } = [];

    public List<Guid>? BottleIds { get; set; }

    public required List<SelectListItem> BottlesOptions { get; set; }
    public required List<SelectListItem> ColorsOptions { get; set; }
}
