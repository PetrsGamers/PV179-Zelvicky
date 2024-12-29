namespace Cap.Enjoyer.WebMVC.Models;

using System.ComponentModel.DataAnnotations;

public class CouponReturnModel
{
    [Required(ErrorMessage = "Code is required.")]
    public required string Code { get; set; }
}
