namespace Cap.Enjoyer.WebMVC.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

public class ProducerCreateViewModel
{
    public string? Name { get; set; }
    public string? City { get; set; }
    public string? Description { get; set; }
    public Guid CountryId { get; set; }
    public required List<SelectListItem> Countries { get; set; }
}
