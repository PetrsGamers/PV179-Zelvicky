namespace Cap.Enjoyer.WebMVC.Models;

using System.ComponentModel.DataAnnotations;
using CapEnjoyer.BL.DTOs;

public class AlbumCreateReturnModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    public required string Description { get; set; }
    public required bool Public { get; set; }
    public required Guid UserId { get; set; }
    public List<CapDto>? Caps { get; set; }
    public List<Guid>? SelectedCapIds { get; set; } = [];
}
