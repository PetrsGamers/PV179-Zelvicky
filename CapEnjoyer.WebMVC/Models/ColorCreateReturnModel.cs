namespace Cap.Enjoyer.WebMVC.Models;

using System.ComponentModel.DataAnnotations;

public class ColorCreateReturnModel
{
    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Please enter a valid hex code in format #A1B2C3.")]
    public required string HexValue { get; set; } = "#";
}
