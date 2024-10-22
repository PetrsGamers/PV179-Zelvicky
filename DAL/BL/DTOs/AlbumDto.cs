namespace DAL.BL.DTOs;

public class AlbumDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Public { get; set; }
    public Guid User { get; set; }
    public List<Guid> Caps { get; set; }
}

