namespace CapEnjoyer.BL.DTOs;

public class CapInsertDto
{
    public string TextOnCap { get; set; }
    public string Description { get; set; }

    public IFormFile? CapPictureFile { get; set; }

    public List<Guid> TextColorIds { get; set; }
    public List<Guid> BgColorIds { get; set; }
    public List<Guid> BottleIds { get; set; }
    public Guid? IsEditForId { get; set; }
}
