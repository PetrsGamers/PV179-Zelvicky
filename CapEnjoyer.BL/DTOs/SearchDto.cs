namespace CapEnjoyer.BL.DTOs;

public class SearchDto
{
    public IEnumerable<BottleDto>? Bottles { get; set; }
    public IEnumerable<ProducerDto>? Producers { get; set; }
    public IEnumerable<CapDto>? Caps { get; set; }
}
