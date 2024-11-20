namespace CapEnjoyer.DAL.Entities;

public class Cap
{
    public Cap() { }

    public Cap(
        Guid? id,
        string textOnCap,
        string description,
        string capPicture,
        List<CapToTextColor>? textColorLinks = null,
        List<CapToBackgroundColor>? backgroundColorLinks = null,
        List<CapToBottle>? bottleLinks = null,
        List<CapToAlbum>? albumLinks = null,
        Guid? isEditForId = null,
        Cap? isEditFor = null,
        List<Cap>? edits = null)
    {
        Id = id ?? Guid.NewGuid();
        TextOnCap = textOnCap;
        Description = description;
        CapPicture = capPicture;
        TextColorLinks = textColorLinks ?? [];
        BackgroundColorLinks = backgroundColorLinks ?? [];
        BottleLinks = bottleLinks ?? [];
        AlbumLinks = albumLinks ?? [];
        this.IsEditForId = isEditForId;
        this.IsEditFor = isEditFor;
        this.Edits = edits ?? [];
    }

    public Guid Id { get; set; }
    public string TextOnCap { get; set; }
    public string Description { get; set; }
    public string CapPicture { get; set; }

    public List<CapToTextColor> TextColorLinks { get; set; }
    public List<CapToBackgroundColor> BackgroundColorLinks { get; set; }
    public List<CapToBottle> BottleLinks { get; set; }
    public List<CapToAlbum> AlbumLinks { get; set; }

    public Guid? IsEditForId { get; set; }
    public Cap? IsEditFor { get; set; }
    public List<Cap> Edits { get; set; }
}
