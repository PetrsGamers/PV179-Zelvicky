namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class Cap
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string TextOnCap { get; set; }
    public required string Description { get; set; }
    public required string CapPicture { get; set; }

    public List<CapToTextColor> TextColorLinks { get; set; } = [];
    public List<CapToBackgroundColor> BackgroundColorLinks { get; set; } = [];
    public List<CapToBottle> BottleLinks { get; set; } = [];
    public List<CapToAlbum> AlbumLinks { get; set; } = [];

    public Guid? IsEditForId { get; set; }

    [ForeignKey("IsEditForId")] public Cap? IsEditFor { get; set; }

    public List<Cap> Edits { get; set; } = [];
}
