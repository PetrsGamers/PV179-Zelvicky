namespace CapEnjoyer.BL.DTOs;

public class ProducerWithDetailsDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Description { get; set; }
    public required string Country { get; set; }
    public Guid? IsEditForId { get; set; }
}
