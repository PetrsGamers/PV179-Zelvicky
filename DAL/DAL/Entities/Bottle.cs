namespace DAL.Entities;
using DAL.Constants;

public class Bottle
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Voltage { get; set; }
    public string BottlePicture { get; set; }

    public DrinkType DrinkType { get; set; }
    public Guid ProducerId { get; set; }
    public Producer Producer { get; set; }

    public List<Cap> Caps { get; set; }

    public Bottle? IsEditFor { get; set; }
    public Guid? IsEditForId { get; set; }
    public List<Bottle> Edits { get; set; }
}
