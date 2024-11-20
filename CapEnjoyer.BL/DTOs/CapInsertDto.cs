namespace CapEnjoyer.BL.DTOs;

public class CapInsertDto
{
    public string TextOnCap { get; set; }
    public string Description { get; set; }
    public string CapPicture { get; set; }
    public List<Guid> TextColors { get; set; }
    public List<Guid> BgColors { get; set; }
    public List<Guid> Bottles { get; set; }
    public Guid? IsEditFor { get; set; }
}
