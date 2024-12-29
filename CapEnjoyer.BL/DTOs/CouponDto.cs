namespace CapEnjoyer.BL.DTOs;

public class CouponDto
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required Guid BuyerId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public Guid? ActivateeId { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
    public bool IsUsed { get; set; }
    public string? ActivateeUsername { get; set; }
}
