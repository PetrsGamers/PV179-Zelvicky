namespace DAL.Entities;

public class Cap
{
    public Guid Id { get; set; }
    public string TextOnCap { get; set; }
    public string Description { get; set; }
    public string CapPicture { get; set; }
    public List<Color> TextColors { get; set; }
    public List<Color> BgColors { get; set; }
    public List<Bottle> Bottles { get; set; }
    public List<Album> Albums { get; set; }
    public Guid? IsEditForId { get; set; }
    public Cap? IsEditFor { get; set; }
    public List<Cap> Edits { get; set; }
}
