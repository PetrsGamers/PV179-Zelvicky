namespace CapEnjoyer.BL.Services;

using DAL;
using DTOs;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

public class EditRequestService(CapEnjoyerDbContext context, IProducerService producerService, IBottleService bottleService, ICapService capService) : IEditRequestService
{
    public async Task<EditRequestsInfoDto<CapWithDetailsDto>> GetCapEditRequestInfo()
    {
        var editCount = await context.Caps.CountAsync(c => c.IsEditFor != null);
        var firstEditRequest = await GetFirstCapEditRequest();
        CapWithDetailsDto? currentEntity = null;

        if (firstEditRequest?.IsEditForId is not null)
        {
            currentEntity = await capService.FindCapWithDetailsByIdAsync(firstEditRequest.IsEditForId.Value);
        }

        return new EditRequestsInfoDto<CapWithDetailsDto>
        {
            EditRequestCount = editCount,
            FirstEditRequest = firstEditRequest,
            CurrentEntity = currentEntity
        };
    }

    public async Task<EditRequestsInfoDto<BottleWithDetailsDto>> GetBottleEditRequestInfo()
    {
        var editCount = await context.Bottles.CountAsync(b => b.IsEditFor != null);
        var firstEditRequest = await GetFirstBottleEditRequest();
        BottleWithDetailsDto? currentEntity = null;

        if (firstEditRequest?.IsEditForId is not null)
        {
            currentEntity = await bottleService.FindBottleWithDetailsByIdAsync(firstEditRequest.IsEditForId.Value);
        }

        return new EditRequestsInfoDto<BottleWithDetailsDto>
        {
            EditRequestCount = editCount,
            FirstEditRequest = firstEditRequest,
            CurrentEntity = currentEntity
        };
    }

    public async Task<EditRequestsInfoDto<ProducerWithDetailsDto>> GetProducerEditRequestInfo()
    {
        var editCount = await context.Producers.CountAsync(p => p.IsEditFor != null);
        var firstEditRequest = await GetFirstProducerEditRequest();
        ProducerWithDetailsDto? currentEntity = null;

        if (firstEditRequest?.IsEditForId is not null)
        {
            currentEntity = await producerService.FindProducerWithDetailsByIdAsync(firstEditRequest.IsEditForId.Value);
        }
        return new EditRequestsInfoDto<ProducerWithDetailsDto>
        {
            EditRequestCount = editCount,
            FirstEditRequest = firstEditRequest,
            CurrentEntity = currentEntity
        };
    }

    private async Task<CapWithDetailsDto?> GetFirstCapEditRequest()
    {
        var firstCap = await context.Caps
            .FirstOrDefaultAsync(c => c.IsEditFor != null);

        if (firstCap is null)
        {
            return null;
        }

        return await capService.FindCapWithDetailsByIdAsync(firstCap.Id);
    }

    private async Task<BottleWithDetailsDto?> GetFirstBottleEditRequest()
    {
        var firstBottleRequest = await context.Bottles
            .FirstOrDefaultAsync(b => b.IsEditFor != null);

        if (firstBottleRequest is null)
        {
            return null;
        }

        return await bottleService.FindBottleWithDetailsByIdAsync(firstBottleRequest.Id);
    }



    private async Task<ProducerWithDetailsDto?> GetFirstProducerEditRequest()
    {
        var firstProducer = await context.Producers.FirstOrDefaultAsync(p => p.IsEditFor != null);
        if (firstProducer is null)
        {
            return null;
        }

        return await producerService.FindProducerWithDetailsByIdAsync(firstProducer.Id);
    }


    public async Task ConfirmCapEdit(Guid currentEntityId, Guid editRequestId, bool isEditConfirmed)
    {
        var currentCap = await capService.GetCapByIdAsync(currentEntityId);
        var capEdit = await capService.GetCapByIdAsync(editRequestId);

        var finalBottle = isEditConfirmed ? capEdit.Adapt<CapInsertDto>() : currentCap.Adapt<CapInsertDto>();
        finalBottle.IsEditForId = null;

        await capService.UpdateCapAsync(currentCap.Id, finalBottle);
        await capService.DeleteCapAsync(capEdit.Id);
        await context.SaveChangesAsync();
    }

    public async Task ConfirmBottleEdit(Guid currentEntityId, Guid editRequestId, bool isEditConfirmed)
    {
        var currentBottle = await bottleService.GetBottleByIdAsync(currentEntityId);
        var bottleEdit = await bottleService.GetBottleByIdAsync(editRequestId);

        var finalBottle = isEditConfirmed ? bottleEdit.Adapt<BottleInsertDto>() : currentBottle.Adapt<BottleInsertDto>();
        finalBottle.IsEditForId = null;

        await bottleService.UpdateBottleAsync(currentBottle.Id, finalBottle);
        await bottleService.DeleteBottleAsync(bottleEdit.Id);
        await context.SaveChangesAsync();
    }

    public async Task ConfirmProducerEdit(Guid currentEntityId, Guid editRequestId, bool isEditConfirmed)
    {
        var currentProducer = await producerService.GetProducerByIdAsync(currentEntityId);
        var producerEdit = await producerService.GetProducerByIdAsync(editRequestId);

        var finalProducer = isEditConfirmed ? producerEdit.Adapt<ProducerInsertDto>() : currentProducer.Adapt<ProducerInsertDto>();
        finalProducer.IsEditForId = null;

        await producerService.UpdateProducerAsync(currentProducer.Id, finalProducer);
        await producerService.DeleteProducerAsync(producerEdit.Id);
        await context.SaveChangesAsync();
    }
}
