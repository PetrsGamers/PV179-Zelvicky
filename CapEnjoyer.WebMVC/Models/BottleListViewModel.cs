namespace Cap.Enjoyer.WebMVC.Models;

public class BottleListViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Voltage { get; set; }
    public string BottlePicture { get; set; }
    public string DrinkType { get; set; }
    public Guid Producer { get; set; }
    public List<Guid> Caps { get; set; }
    public Guid? IsEditFor { get; set; }
}
