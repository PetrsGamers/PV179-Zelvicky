namespace Cap.Enjoyer.WebMVC.Models;

public class ProducerCreateViewModel
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Description { get; set; }
    public Guid Country { get; set; }
    public Guid? IsEditFor { get; set; }
}
