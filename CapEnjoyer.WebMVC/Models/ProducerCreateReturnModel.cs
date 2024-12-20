namespace Cap.Enjoyer.WebMVC.Models;

using System.ComponentModel.DataAnnotations;
using CapEnjoyer.DAL.Entities;

public class ProducerCreateReturnModel
{
    [Required(ErrorMessage = "Name is required.")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "City is required.")]
    public required string City { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    public required string Description { get; set; }
    [Required(ErrorMessage = "Country is required.")]
    public Guid CountryId { get; set; }
}
