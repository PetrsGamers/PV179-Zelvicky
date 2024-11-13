namespace CapEnjoyer.DAL.Entities;

public class Country
{
    public Country() { }

    public Country(Guid? id, string name, List<Producer> producers)
    {
        this.Id = id ?? Guid.NewGuid();
        this.Name = name;
        this.Producers = producers;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Producer> Producers { get; set; }
}
