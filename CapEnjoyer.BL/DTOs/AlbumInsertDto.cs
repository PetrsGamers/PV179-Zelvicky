namespace CapEnjoyer.BL.DTOs;

public class AlbumInsertDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Public { get; set; }

    public Guid UserId { get; set; }
    public List<Guid> CapsIds { get; set; }
}
