namespace CapEnjoyer.DAL.Entities;

public class Album
{
    public Album() { }

    public Album(Guid? id, string name, string description, bool @public, Guid userId, User user,
        List<CapToAlbum>? capLinks = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Description = description;
        Public = @public;
        UserId = userId;
        User = user;
        CapLinks = capLinks ?? [];
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Public { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public List<CapToAlbum> CapLinks { get; set; }
}
