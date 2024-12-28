namespace Cap.Enjoyer.WebMVC.Models;

using System.ComponentModel.DataAnnotations;

public class BottleCreateReturnModel
{
    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Voltage is required.")]
    public required double Voltage { get; set; }

    [Required(ErrorMessage = "DrinkType is required.")]
    public required string DrinkType { get; set; }

    [Required(ErrorMessage = "Producer is required.")]
    public required Guid ProducerId { get; set; }

    public IFormFile? BottlePicture { get; set; }
    public required List<Guid>? CapIds { get; set; }
}
