namespace Cap.Enjoyer.WebMVC.Models;

using System.ComponentModel.DataAnnotations;

public class CapCreateReturnModel
{
    [Required(ErrorMessage = "Text on cap is required.")]
    [StringLength(100)]
    public required string TextOnCap { get; set; } = string.Empty;

    [StringLength(500)]
    [Required(ErrorMessage = "Description is required.")]
    public required string Description { get; set; }

    public IFormFile? CapPictureFile { get; set; }

    public List<Guid>? TextColorIds { get; set; } = [];

    public List<Guid> BgColorIds { get; set; } = [];

    public List<Guid>? BottleIds { get; set; }
}
