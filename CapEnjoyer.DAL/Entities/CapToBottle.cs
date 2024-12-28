namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class CapToBottle
{
    public required Guid CapId { get; set; }

    [ForeignKey("CapId")] public required Cap Cap { get; set; }

    public required Guid BottleId { get; set; }

    [ForeignKey("BottleId")] public Bottle? Bottle { get; set; }
}
