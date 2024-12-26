namespace CapEnjoyer.DAL.Entities;

public class Color
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string HexCode { get; set; }
    public List<CapToTextColor> CapTextLinks { get; set; } = [];
    public List<CapToBackgroundColor> CapBackgroundLinks { get; set; } = [];
}
