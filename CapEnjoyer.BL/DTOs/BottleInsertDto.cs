namespace CapEnjoyer.BL.DTOs;

public class BottleInsertDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public double Voltage { get; set; }

    public IFormFile? BottlePictureFile { get; set; }
    public string DrinkType { get; set; }
    public Guid ProducerId { get; set; }
    public List<Guid>? Caps { get; set; }
    public Guid? IsEditForId { get; set; }
}
