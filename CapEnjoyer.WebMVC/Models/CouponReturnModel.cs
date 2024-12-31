namespace Cap.Enjoyer.WebMVC.Models;

using System.ComponentModel.DataAnnotations;

public class CouponReturnModel
{
    [Required(ErrorMessage = "Code is required.")]
    [StringLength(64)]
    public required string Code { get; set; }
}
