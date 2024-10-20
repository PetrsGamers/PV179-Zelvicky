namespace DAL.Entities;
using DAL.Constants;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Album> Albums { get; set; }
    public Role Role { get; set; }
}
