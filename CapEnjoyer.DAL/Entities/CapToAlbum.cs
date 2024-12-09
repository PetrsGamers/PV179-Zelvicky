namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class CapToAlbum
{
    public Guid CapId { get; set; }
    [ForeignKey("CapId")]
    public Cap Cap { get; set; }

    public Guid AlbumId { get; set; }
    [ForeignKey("AlbumId")]
    public Album Album { get; set; }
}
