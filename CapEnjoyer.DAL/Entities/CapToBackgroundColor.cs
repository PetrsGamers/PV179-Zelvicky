namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class CapToBackgroundColor
{
    public required Guid CapId { get; set; }

    [ForeignKey("CapId")] public required Cap Cap { get; set; }

    public required Guid BackgroundColorId { get; set; }

    [ForeignKey("BackgroundColorId")] public Color? BackgroundColor { get; set; }
}
