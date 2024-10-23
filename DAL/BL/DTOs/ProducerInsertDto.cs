namespace DAL.BL.DTOs;

public class ProducerInsertDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public Guid Country { get; set; }
    public Guid? IsEditFor { get; set; }
}
