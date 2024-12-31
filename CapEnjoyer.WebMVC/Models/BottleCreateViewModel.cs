namespace Cap.Enjoyer.WebMVC.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

public class BottleCreateViewModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Voltage { get; set; }
    public IFormFile? BottlePictureFile { get; set; }
    public string? DrinkType { get; set; }
    public Guid? ProducerId { get; set; }
    public List<Guid>? CapIds { get; set; }

    public required List<SelectListItem>? ProducersOptions { get; set; }
    public required List<SelectListItem>? DrinkTypes { get; set; }
    public required List<SelectListItem>? CapsOptions { get; set; }
}
