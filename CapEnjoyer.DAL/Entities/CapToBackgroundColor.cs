namespace CapEnjoyer.DAL.Entities;

public class CapToBackgroundColor
{
    public CapToBackgroundColor()
    {
    }

    public CapToBackgroundColor(Guid capId, Cap cap, Guid backgroundColorId, Color backgroundColor)
    {
        this.CapId = capId;
        this.Cap = cap;
        this.BackgroundColorId = backgroundColorId;
        this.BackgroundColor = backgroundColor;
    }

    public Guid CapId { get; set; }
    public Cap Cap { get; set; }

    public Guid BackgroundColorId { get; set; }
    public Color BackgroundColor { get; set; }
}
