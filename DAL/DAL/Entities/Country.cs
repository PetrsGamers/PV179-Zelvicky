namespace DAL.Entities;

public class Country
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Producer> Producers { get; set; }
}
