namespace DAL.Entities;

public class Album
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Public { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public List<Cap> Caps { get; set; }
}
