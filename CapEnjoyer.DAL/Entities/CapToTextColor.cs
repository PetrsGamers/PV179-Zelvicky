namespace CapEnjoyer.DAL.Entities;

public class CapToTextColor
{
    public CapToTextColor()
    {
    }

    public CapToTextColor(Guid capId, Cap cap, Guid textColorId, Color textColor)
    {
        this.CapId = capId;
        this.Cap = cap;
        this.TextColorId = textColorId;
        this.TextColor = textColor;
    }

    public Guid CapId { get; set; }
    public Cap Cap { get; set; }

    public Guid TextColorId { get; set; }
    public Color TextColor { get; set; }
}
