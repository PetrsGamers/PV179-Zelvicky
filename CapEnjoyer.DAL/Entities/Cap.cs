namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class Cap
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string TextOnCap { get; set; }
    public required string Description { get; set; }
    public required string CapPicture { get; set; }

    public List<CapToTextColor> TextColorLinks { get; set; } = new List<CapToTextColor>();
    public List<CapToBackgroundColor> BackgroundColorLinks { get; set; } = new List<CapToBackgroundColor>();
    public List<CapToBottle> BottleLinks { get; set; } = new List<CapToBottle>();
    public List<CapToAlbum> AlbumLinks { get; set; } = new List<CapToAlbum>();

    public Guid? IsEditForId { get; set; }
    [ForeignKey("IsEditForId")]
    public Cap? IsEditFor { get; set; }
    public List<Cap> Edits { get; set; } = new List<Cap>();
}
