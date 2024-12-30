namespace Cap.Enjoyer.WebMVC.Models;

public class EditRequestBottleViewModel
{
    public required int RequestCount { get; set; }
    public required BottleDetailViewModel OriginalBottle { get; set; }
    public required BottleDetailViewModel EditRequestBottle { get; set; }
}
