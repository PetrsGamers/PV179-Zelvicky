namespace Cap.Enjoyer.WebMVC.Models;

public class AlbumListViewModel
{
    public List<AlbumViewModel> Albums { get; set; }
    public Guid? LoggedUser { get; set; }
}
