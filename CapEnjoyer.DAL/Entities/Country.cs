namespace CapEnjoyer.DAL.Entities;

public record Country
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Producer> Producers { get; set; }
}
