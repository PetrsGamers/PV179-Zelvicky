namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface IEditRequestService
{
    public Task<EditRequestsInfoDto> GetEditRequestInfo();
    public Task ConfirmCapEdit(Guid currentCapId, Guid capRequestId, bool isEditConfirmed);
    public Task ConfirmBottleEdit(Guid currentBottleId, Guid bottleRequestId, bool isEditConfirmed);
    public Task ConfirmProducerEdit(Guid currentProducerId, Guid producerRequestId, bool isEditConfirmed);
}
