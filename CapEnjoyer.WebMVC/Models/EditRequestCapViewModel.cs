namespace Cap.Enjoyer.WebMVC.Models;

public class EditRequestCapViewModel
{
    public required int RequestCount { get; set; }
    public required CapDetailViewModel OriginalCap { get; set; }
    public required CapDetailViewModel EditRequestCap { get; set; }
}
