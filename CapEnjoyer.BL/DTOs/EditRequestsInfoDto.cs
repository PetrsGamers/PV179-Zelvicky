namespace CapEnjoyer.BL.DTOs;

public class EditRequestsInfoDto
{
    public int CapEditRequestCount { get; set; }
    public int BottleEditRequestCount { get; set; }
    public int ProducerEditRequestCount { get; set; }
    public CapDto? FirstCapEditRequest { get; set; }
    public BottleDto? FirstBottleEditRequest { get; set; }
    public ProducerDto? FirstProducerEditRequest { get; set; }
    public CapDto? CurrentCap { get; set; }
    public BottleDto? CurrentBottle { get; set; }
    public ProducerDto? CurrentProducer { get; set; }
}
