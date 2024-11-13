namespace CapEnjoyer.DAL.Entities;

public class CapToBottle
{
    public CapToBottle()
    {
    }

    public CapToBottle(Guid capId, Cap cap, Guid bottleId, Bottle bottle)
    {
        this.CapId = capId;
        this.Cap = cap;
        this.BottleId = bottleId;
        this.Bottle = bottle;
    }

    public Guid CapId { get; set; }
    public Cap Cap { get; set; }

    public Guid BottleId { get; set; }
    public Bottle Bottle { get; set; }
}
