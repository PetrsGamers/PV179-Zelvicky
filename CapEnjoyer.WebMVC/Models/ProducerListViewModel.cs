namespace Cap.Enjoyer.WebMVC.Models;

public class ProducerListViewModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Description { get; set; }
    public required Guid CountryId { get; set; }
}
