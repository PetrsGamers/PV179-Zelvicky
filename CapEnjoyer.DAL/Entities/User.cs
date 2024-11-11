namespace CapEnjoyer.DAL.Entities;

using Constants;

public class User
{
    public User() { }

    public User(Guid? id, string username, string email, string password, List<Album> albums, Role role)
    {
        this.Id = id ?? Guid.NewGuid();
        this.Username = username;
        this.Email = email;
        this.Password = password;
        this.Albums = albums ?? new List<Album>(); // Default empty list if null
        this.Role = role;
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Album> Albums { get; set; }
    public Role Role { get; set; }
}
