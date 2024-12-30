namespace CapEnjoyer.BL.DTOs;

public class BottleWithDetailsDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required double Voltage { get; set; }
    public required string BottlePicture { get; set; }
    public required string DrinkType { get; set; }
    public required ProducerDto Producer { get; set; }
    public required List<CapDto> CapDetails { get; set; }
    public Guid? IsEditForId { get; set; }

}
