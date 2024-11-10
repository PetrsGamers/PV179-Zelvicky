namespace CapEnjoyer.DAL.Entities;

public class Country
{
    public Country() { }

    public Country(Guid? id, string name, List<Producer> producers)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Producers = producers;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Producer> Producers { get; set; }
}
