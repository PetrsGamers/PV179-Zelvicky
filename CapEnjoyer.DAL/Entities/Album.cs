namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class Album
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool Public { get; set; }
    public required Guid UserId { get; set; }

    [ForeignKey("UserId")] public required User User { get; set; }

    public List<CapToAlbum> CapLinks { get; set; } = [];
}
