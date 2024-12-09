namespace CapEnjoyer.DAL.Entities;

public class Country
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public List<Producer> Producers { get; set; } = [];
}
