namespace CapEnjoyer.BL.DTOs;

public class CapWithDetailsDto
{
    public required Guid Id { get; set; }
    public required string TextOnCap { get; set; }
    public required string Description { get; set; }
    public string? CapPicture { get; set; }
    public required List<ColorDto> TextColors { get; set; }
    public required List<ColorDto> BgColors { get; set; }
    public required List<BottleDto> Bottles { get; set; }
    public Guid? IsEditForId { get; set; }
}

