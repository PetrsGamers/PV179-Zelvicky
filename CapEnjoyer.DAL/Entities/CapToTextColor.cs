namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class CapToTextColor
{
    public required Guid CapId { get; set; }
    [ForeignKey("CapId")]
    public required Cap Cap { get; set; }

    public required Guid TextColorId { get; set; }
    [ForeignKey("TextColorId")]
    public Color? TextColor { get; set; }
}
