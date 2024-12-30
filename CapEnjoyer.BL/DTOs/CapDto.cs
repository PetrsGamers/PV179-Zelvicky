namespace CapEnjoyer.BL.DTOs;

public class CapDto
{
    public Guid Id { get; set; }
    public string TextOnCap { get; set; }
    public string Description { get; set; }
    public string CapPicture { get; set; }
    public List<Guid> TextColorIds { get; set; }
    public List<Guid> BgColorIds { get; set; }
    public List<Guid> BottleIds { get; set; }
    public Guid? IsEditForId { get; set; }
}
