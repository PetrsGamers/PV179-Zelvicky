namespace Cap.Enjoyer.WebMVC.Models;

public class CouponViewModel
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? ActivateeUsername { get; set; }
    public string? BuyerUsername { get; set; }
    public DateTime? GeneratedAt { get; set; }
}
