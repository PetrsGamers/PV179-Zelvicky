namespace CapEnjoyer.BL.DTOs;

public class ProducerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public Guid CountryId { get; set; }
    public Guid? IsEditForId { get; set; }
}
