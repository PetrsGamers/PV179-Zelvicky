namespace CapEnjoyer.BL.DTOs;

public class BottleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Voltage { get; set; }
    public string BottlePicture { get; set; }
    public string DrinkType { get; set; }
    public Guid ProducerId { get; set; }
    public List<Guid>? CapIds { get; set; }
    public Guid? IsEditForId { get; set; }
}
