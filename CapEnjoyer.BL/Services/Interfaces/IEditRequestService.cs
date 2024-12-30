namespace CapEnjoyer.BL.Services.Interfaces;

using CapEnjoyer.BL.DTOs;

public interface IEditRequestService
{
    Task<EditRequestsInfoDto<CapWithDetailsDto>> GetCapEditRequestInfo();
    Task ConfirmCapEdit(Guid currentEntityId, Guid editRequestId, bool isEditConfirmed);

    Task<EditRequestsInfoDto<BottleWithDetailsDto>> GetBottleEditRequestInfo();
    Task ConfirmBottleEdit(Guid currentEntityId, Guid editRequestId, bool isEditConfirmed);

    Task<EditRequestsInfoDto<ProducerWithDetailsDto>> GetProducerEditRequestInfo();
    Task ConfirmProducerEdit(Guid currentEntityId, Guid editRequestId, bool isEditConfirmed);
}
