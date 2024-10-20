namespace DAL.Entities;

public class Producer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public Guid CountryId { get; set; }
    public Country Country { get; set; }


    public Producer? IsEditFor { get; set; }
    public Guid? IsEditForId { get; set; }
    public List<Producer> Edits { get; set; }
}
