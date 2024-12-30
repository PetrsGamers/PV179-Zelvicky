namespace Cap.Enjoyer.WebMVC.Models;

using CapEnjoyer.BL.DTOs;

public class BottleDetailViewModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required double Voltage { get; set; }
    public required string BottlePicture { get; set; }
    public required string DrinkType { get; set; }
    public required ProducerDto Producer { get; set; }
    public required List<CapDto> CapDetails { get; set; }
    public Guid? IsEditFor { get; set; }
}
