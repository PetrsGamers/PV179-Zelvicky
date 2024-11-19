namespace CapEnjoyer.BL.DTOs;

public class ConfirmEditRequestDto

{
    public Guid CurrentId { get; set; }
    public Guid RequestId { get; set; }
    public bool IsEditConfirmed { get; set; }
}
