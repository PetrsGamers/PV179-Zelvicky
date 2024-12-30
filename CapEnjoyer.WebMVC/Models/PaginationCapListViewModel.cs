namespace Cap.Enjoyer.WebMVC.Models;

public class PaginationCapListViewModel
{
    public IEnumerable<CapDetailViewModel> Caps { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

}
