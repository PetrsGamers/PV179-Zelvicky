namespace CapEnjoyer.DAL.Entities;

using Constants;

public class User
{
    public User() { }

    public User(Guid? id, string username, string email, Role role, List<Album>? albums = null)
    {
        Id = id ?? Guid.NewGuid();
        Username = username;
        Email = email;
        Albums = albums ?? [];
        Role = role;
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<Album> Albums { get; set; }
    public Role Role { get; set; }
}
