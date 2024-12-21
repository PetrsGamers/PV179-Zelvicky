namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class CapToAlbum
{
    public required Guid CapId { get; set; }
    [ForeignKey("CapId")]
    public required Cap Cap { get; set; }

    public required Guid AlbumId { get; set; }
    [ForeignKey("AlbumId")]
    public Album Album { get; set; }
}
