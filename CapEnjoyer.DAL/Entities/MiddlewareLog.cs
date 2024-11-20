namespace CapEnjoyer.DAL.Entities;

using Constants;

public class MiddlewareLog
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public MiddlewareLogAction Action { get; set; }
    public string Log { get; set; }
}
