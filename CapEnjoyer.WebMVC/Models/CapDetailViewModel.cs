namespace Cap.Enjoyer.WebMVC.Models;

public class CapDetailViewModel
{
    public required Guid Id { get; set; }
    public required string TextOnCap { get; set; }
    public required string Description { get; set; }
    public string? CapPicture { get; set; }
    public required List<ColorDetail> TextColors { get; set; }
    public required List<ColorDetail> BgColors { get; set; }
    public required List<BottleDetail> Bottles { get; set; }
    public Guid? IsEditForId { get; set; }
}

public class ColorDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string HexCode { get; set; }
}

public class BottleDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
