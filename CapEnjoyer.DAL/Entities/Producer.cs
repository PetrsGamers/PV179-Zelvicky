namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class Producer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Description { get; set; }
    public required Guid CountryId { get; set; }
    public required Country Country { get; set; }
    public Producer? IsEditFor { get; set; }
    [ForeignKey("IsEditForId")]
    public Guid? IsEditForId { get; set; }
    public List<Producer> Edits { get; set; } = new List<Producer>();
}
