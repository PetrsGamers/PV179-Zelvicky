namespace CapEnjoyer.DAL.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class Coupon
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; }
    public required Guid BuyerId { get; set; }

    [ForeignKey("BuyerId")] public User Buyer { get; set; }

    public Guid? ActivateeId { get; set; }

    [ForeignKey("ActivateeId")] public User? Activatee { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.Now.ToUniversalTime();
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
    public bool IsUsed { get; set; }
}
