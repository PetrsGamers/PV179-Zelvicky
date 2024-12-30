namespace Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.DTOs;

public class AlbumCreateViewModel
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Public { get; set; }
    public Guid? UserId { get; set; }
    public List<CapDto>? Caps { get; set; }
    public List<Guid>? SelectedCapIds { get; set; } = [];
}
