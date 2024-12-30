namespace Cap.Enjoyer.WebMVC.Models;

public class EditRequestConfirmationModel
{
    public required Guid CurrentEntityId { get; set; }
    public required Guid EditRequestId { get; set; }
    public required bool IsEditConfirmed { get; set; }
}
