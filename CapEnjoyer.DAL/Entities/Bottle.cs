namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using Constants;

public class Bottle
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required double Voltage { get; set; }
    public required DrinkType DrinkType { get; set; }
    public required string BottlePicture { get; set; }
    public required Guid ProducerId { get; set; }

    [ForeignKey("ProducerId")] public required Producer Producer { get; set; }

    public List<CapToBottle> CapLinks { get; set; } = [];

    public Guid? IsEditForId { get; set; }

    [ForeignKey("IsEditForId")] public Bottle? IsEditFor { get; set; }

    public List<Bottle> Edits { get; set; } = [];
}
