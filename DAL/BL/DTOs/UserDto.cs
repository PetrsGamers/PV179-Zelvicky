namespace DAL.BL.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public List<AlbumDto> Albums { get; set; }

    public string Role { get; set; }
}
