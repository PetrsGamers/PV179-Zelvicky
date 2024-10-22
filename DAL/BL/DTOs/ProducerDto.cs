namespace DAL.BL.DTOs;

public class ProducerDto{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public Guid Country { get; set; }
    public Guid? IsEditFor { get; set; }
}
