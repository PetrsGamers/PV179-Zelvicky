namespace CapEnjoyer.DAL.Entities;

using Constants;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public required string Email { get; set; }
    public List<Album> Albums { get; set; } = [];
    public required Role Role { get; set; }
}
