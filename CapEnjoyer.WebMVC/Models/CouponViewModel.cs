namespace Cap.Enjoyer.WebMVC.Models;

public class CouponViewModel
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string? ActivateeName { get; set; }
    public string? BuyerName { get; set; }
    public DateTime? BoughtAt { get; set; }
}
