namespace CapEnjoyer.BL.DTOs;

public class EditRequestsInfoDto<T>
{
    public int EditRequestCount { get; set; }
    public T? FirstEditRequest { get; set; }
    public T? CurrentEntity { get; set; }
}
