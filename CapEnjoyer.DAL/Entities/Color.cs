namespace CapEnjoyer.DAL.Entities;

public record Color
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string HexCode { get; set; }

    public List<Cap> CapTexts { get; } = [];
    public List<Cap> CapBackgrounds { get; } = [];
}
