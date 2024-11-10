namespace CapEnjoyer.DAL.Entities;

public class Color
{
    public Color() { }

    public Color(Guid? id, string name, string hexCode, List<CapToTextColor>? capTextLinks = null, List<CapToBackgroundColor>? capBackgroundLinks = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        HexCode = hexCode;
        CapTextLinks = capTextLinks ?? new List<CapToTextColor>();
        CapBackgroundLinks = capBackgroundLinks ?? new List<CapToBackgroundColor>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string HexCode { get; set; }
    public List<CapToTextColor> CapTextLinks { get; set; }
    public List<CapToBackgroundColor> CapBackgroundLinks { get; set; }
}
