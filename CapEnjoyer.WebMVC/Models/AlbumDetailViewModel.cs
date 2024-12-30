namespace Cap.Enjoyer.WebMVC.Models;

using CapEnjoyer.BL.DTOs;

public class AlbumDetailViewModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool Public { get; set; }
    public required string Username { get; set; }
    public List<CapDto>? Caps { get; set; } = [];
}
