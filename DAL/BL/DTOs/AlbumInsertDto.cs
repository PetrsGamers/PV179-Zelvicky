namespace DAL.BL.DTOs;

public class AlbumInsertDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Public { get; set; }
    public Guid User { get; set; }
    public List<Guid> Caps { get; set; }
}

