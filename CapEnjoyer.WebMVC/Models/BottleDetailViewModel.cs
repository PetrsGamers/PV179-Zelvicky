namespace Cap.Enjoyer.WebMVC.Models;

using CapEnjoyer.BL.DTOs;

public class BottleDetailViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Voltage { get; set; }
    public string BottlePicture { get; set; }
    public string DrinkType { get; set; }
    public Guid Producer { get; set; }
    public string ProducerName { get; set; }
    public List<CapDto> CapDetails { get; set; }
    public Guid? IsEditFor { get; set; }
}

