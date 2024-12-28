namespace Cap.Enjoyer.WebMVC.Models;

public class CapViewModel
{
    public required Guid Id { get; set; }
    public required string TextOnCap { get; set; }
    public required string Description { get; set; }
    public string? CapPicture { get; set; }
    public required List<Guid> TextColorsIds { get; set; }
    public required List<Guid> BgColorsIds { get; set; }
    public required List<Guid> BottlesIds { get; set; }
    public Guid? IsEditFor { get; set; }
}
