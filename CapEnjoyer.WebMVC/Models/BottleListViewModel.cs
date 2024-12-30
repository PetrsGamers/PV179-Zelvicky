namespace Cap.Enjoyer.WebMVC.Models;

public class BottleListViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Voltage { get; set; }
    public string? BottlePicture { get; set; }
    public required string DrinkType { get; set; }
    public required Guid Producer { get; set; }
    public List<Guid>? Caps { get; set; }
    public Guid? IsEditFor { get; set; }
}
