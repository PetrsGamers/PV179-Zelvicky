namespace CapEnjoyer.DAL.Entities;

public class Producer
{
    public Producer() { }

    public Producer(
        Guid? id,
        string name,
        string city,
        string description,
        Guid countryId,
        Country country,
        Producer? isEditFor,
        Guid? isEditForId,
        List<Producer>? edits = null)
    {
        this.Id = id ?? Guid.NewGuid();
        this.Name = name;
        this.City = city;
        this.Description = description;
        this.CountryId = countryId;
        this.Country = country;
        this.IsEditFor = isEditFor;
        this.IsEditForId = isEditForId;
        this.Edits = edits ?? [];
    }

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
