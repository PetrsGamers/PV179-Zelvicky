namespace Cap.Enjoyer.WebMVC.Models;

public class AlbumViewModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool Public { get; set; }
    public required string Username { get; set; }
    public required Guid UserId { get; set; }
    public List<Guid>? Caps { get; set; } = [];
}
