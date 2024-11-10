namespace CapEnjoyer.DAL.Entities;

using Constants;

public class Bottle
{
    public Bottle() { }

    public Bottle(
        Guid? id,
        string name,
        string description,
        double voltage,
        DrinkType drinkType,
        string bottlePicture,
        Guid producerId,
        Producer producer,
        Bottle? isEditFor,
        Guid? isEditForId,
        List<Bottle>? edits = null,
        List<CapToBottle>? capLinks = null)
    {
        this.Id = id ?? Guid.NewGuid();
        this.Name = name;
        this.Description = description;
        this.Voltage = voltage;
        this.DrinkType = drinkType;
        this.BottlePicture = bottlePicture;
        this.ProducerId = producerId;
        this.Producer = producer;
        this.IsEditFor = isEditFor;
        this.IsEditForId = isEditForId;
        this.Edits = edits ?? [];
        this.CapLinks = capLinks ?? [];
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Voltage { get; set; }
    public DrinkType DrinkType { get; set; }
    public string BottlePicture { get; set; }
    public Guid ProducerId { get; set; }
    public Producer Producer { get; set; }
    public List<CapToBottle> CapLinks { get; set; }
    public Guid? IsEditForId { get; set; }
    public Bottle? IsEditFor { get; set; }
    public List<Bottle> Edits { get; set; }
}
