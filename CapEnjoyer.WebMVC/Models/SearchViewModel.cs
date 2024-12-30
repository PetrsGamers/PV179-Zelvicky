namespace Cap.Enjoyer.WebMVC.Models;

using CapEnjoyer.BL.DTOs;

public class SearchViewModel
{
    public required string SearchField { get; set; }
    public IEnumerable<BottleDto>? Bottles { get; set; }
    public IEnumerable<ProducerDto>? Producers { get; set; }
    public IEnumerable<CapDto>? Caps { get; set; }
}
