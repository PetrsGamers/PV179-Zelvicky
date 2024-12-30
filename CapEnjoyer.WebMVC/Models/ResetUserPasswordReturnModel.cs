namespace Cap.Enjoyer.WebMVC.Models;
public class ResetUserPasswordReturnModel
{
    public required string Email { get; set; }
    public required string NewPassword { get; set; }
}
