namespace CapEnjoyer.DAL.Entities;

public class CapToAlbum
{
    public CapToAlbum()
    {
    }

    public CapToAlbum(Guid capId, Cap cap, Guid albumId, Album album)
    {
        this.CapId = capId;
        this.Cap = cap;
        this.AlbumId = albumId;
        this.Album = album;
    }

    public Guid CapId { get; set; }
    public Cap Cap { get; set; }

    public Guid AlbumId { get; set; }
    public Album Album { get; set; }
}
