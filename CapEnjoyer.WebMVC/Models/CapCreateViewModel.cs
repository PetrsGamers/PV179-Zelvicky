namespace Cap.Enjoyer.WebMVC.Models;

public class CapCreateViewModel
{
    public string TextOnCap { get; set; }
    public string Description { get; set; }
    public string CapPicture { get; set; }
    public List<Guid> TextColors { get; set; }
    public List<Guid> BgColors { get; set; }
    public List<Guid> Bottles { get; set; }
    public Guid? IsEditFor { get; set; }
}
