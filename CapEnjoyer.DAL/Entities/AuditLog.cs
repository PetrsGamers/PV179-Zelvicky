namespace CapEnjoyer.DAL.Entities;

using Constants;

public class AuditLog
{
    public Guid Id { get; set; }
    public DateTime EditedAt { get; set; }
    public AuditLogAction Action { get; set; }
    public string Log { get; set; }
    public Guid? UserId { get; set; }
    public Guid CapId { get; set; }
}
