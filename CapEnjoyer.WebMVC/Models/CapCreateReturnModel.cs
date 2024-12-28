namespace Cap.Enjoyer.WebMVC.Models;

using System.ComponentModel.DataAnnotations;

public class CapCreateReturnModel
{
    [Required(ErrorMessage = "Text on cap is required.")]
    [StringLength(100)]
    public string TextOnCap { get; set; } = string.Empty;

    [StringLength(500)]
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }

    public IFormFile? CapPicture { get; set; }


    [Required(ErrorMessage = "Text color is required.")]
    public List<Guid> TextColorsIds { get; set; } = [];

    [Required(ErrorMessage = "Background color is required.")]
    public List<Guid> BgColorsIds { get; set; } = [];

    public List<Guid>? BottlesIds { get; set; }
}
