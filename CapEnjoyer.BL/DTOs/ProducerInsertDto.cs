namespace CapEnjoyer.BL.DTOs;

public class ProducerInsertDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public Guid CountryId { get; set; }
    public Guid? IsEditForId { get; set; }
}
