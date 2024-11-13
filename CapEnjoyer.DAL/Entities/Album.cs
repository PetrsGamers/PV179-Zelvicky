namespace CapEnjoyer.DAL.Entities;

public class Album
{
    public Album() { }

    public Album(Guid? id, string name, string description, bool @public, Guid userId, User user,
        List<CapToAlbum>? capLinks = null)
    {
        this.Id = id ?? Guid.NewGuid();
        this.Name = name;
        this.Description = description;
        this.Public = @public;
        this.UserId = userId;
        this.User = user;
        this.CapLinks = capLinks ?? [];
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Public { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public List<CapToAlbum> CapLinks { get; set; }
}
