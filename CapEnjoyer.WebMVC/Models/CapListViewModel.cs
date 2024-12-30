namespace Cap.Enjoyer.WebMVC.Models;

public class CapListViewModel
{
    public required Guid Id { get; set; }
    public required string TextOnCap { get; set; }
    public required string Description { get; set; }
    public string? CapPicture { get; set; }
    public required List<Guid> TextColorIds { get; set; }
    public required List<Guid> BgColorIds { get; set; }
    public required List<Guid> BottleIds { get; set; }
    public Guid? IsEditFor { get; set; }
}
