namespace CapEnjoyer.BL.Services;
using CapEnjoyer.DAL.Entities;

using DAL;
using DTOs;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class EditRequestService(CapEnjoyerDbContext context) : IEditRequestService
{
    public async Task<EditRequestsInfoDto> GetEditRequestInfo()
    {
        var capEditCount = await context.Caps.CountAsync(c => c.IsEditFor != null);
        var bottleEditCount = await context.Bottles.CountAsync(b => b.IsEditFor != null);
        var producerEditCount = await context.Producers.CountAsync(p => p.IsEditFor != null);

        var firstCapEditRequest = await context.Caps
            .Where(c => c.IsEditFor != null)
            .Select(c => new CapDto
            {
                Id = c.Id,
                TextOnCap = c.TextOnCap,
                Description = c.Description,
                CapPicture = c.CapPicture,
                TextColors = c.TextColorLinks.Select(ctc => ctc.TextColorId).ToList(),
                BgColors = c.BackgroundColorLinks.Select(cbc => cbc.BackgroundColorId).ToList(),
                Bottles = c.BottleLinks.Select(cb => cb.BottleId).ToList(),
                IsEditFor = c.IsEditForId
            })
            .FirstOrDefaultAsync();

        var firstBottleEditRequest = await context.Bottles
            .Where(b => b.IsEditFor != null)
            .Select(b => new BottleDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Voltage = b.Voltage,
                BottlePicture = b.BottlePicture,
                DrinkType = b.DrinkType.ToString(),
                ProducerId = b.ProducerId,
                Caps = b.CapLinks.Select(cb => cb.CapId).ToList(),
                IsEditFor = b.IsEditForId
            })
            .FirstOrDefaultAsync();

        var firstProducerEditRequest = await context.Producers
            .Where(p => p.IsEditFor != null)
            .Select(p => new ProducerDto
            {
                Id = p.Id,
                Name = p.Name,
                City = p.City,
                Description = p.Description,
                Country = p.CountryId,
                IsEditFor = p.IsEditForId
            })
            .FirstOrDefaultAsync();

        var currentCap = firstCapEditRequest?.IsEditFor != null
            ? await context.Caps
                .Where(c => c.Id == firstCapEditRequest.IsEditFor)
                .Select(c => new CapDto
                {
                    Id = c.Id,
                    TextOnCap = c.TextOnCap,
                    Description = c.Description,
                    CapPicture = c.CapPicture,
                    TextColors = c.TextColorLinks.Select(ctc => ctc.TextColorId).ToList(),
                    BgColors = c.BackgroundColorLinks.Select(cbc => cbc.BackgroundColorId).ToList(),
                    Bottles = c.BottleLinks.Select(cb => cb.BottleId).ToList(),
                    IsEditFor = c.IsEditForId
                })
                .FirstOrDefaultAsync()
            : null;

        var currentBottle = firstBottleEditRequest?.IsEditFor != null
            ? await context.Bottles
                .Where(b => b.Id == firstBottleEditRequest.IsEditFor)
                .Select(b => new BottleDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Voltage = b.Voltage,
                    BottlePicture = b.BottlePicture,
                    DrinkType = b.DrinkType.ToString(),
                    ProducerId = b.ProducerId,
                    Caps = b.CapLinks.Select(cb => cb.CapId).ToList(),
                    IsEditFor = b.IsEditForId
                })
                .FirstOrDefaultAsync()
            : null;

        var currentProducer = firstProducerEditRequest?.IsEditFor != null
            ? await context.Producers
                .Where(p => p.Id == firstProducerEditRequest.IsEditFor)
                .Select(p => new ProducerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    City = p.City,
                    Description = p.Description,
                    Country = p.CountryId,
                    IsEditFor = p.IsEditForId
                })
                .FirstOrDefaultAsync()
            : null;

        return new EditRequestsInfoDto
        {
            CapEditRequestCount = capEditCount,
            BottleEditRequestCount = bottleEditCount,
            ProducerEditRequestCount = producerEditCount,
            FirstCapEditRequest = firstCapEditRequest,
            FirstBottleEditRequest = firstBottleEditRequest,
            FirstProducerEditRequest = firstProducerEditRequest,
            CurrentCap = currentCap,
            CurrentBottle = currentBottle,
            CurrentProducer = currentProducer
        };
    }

    public async Task ConfirmCapEdit(Guid currentCapId, Guid capRequestId, bool isEditConfirmed)
    {
        var capRequest = await context.Caps
            .Include(c => c.TextColorLinks)
                .ThenInclude(tcl => tcl.TextColor)
            .Include(c => c.BackgroundColorLinks)
                .ThenInclude(bcl => bcl.BackgroundColor)
            .Include(c => c.BottleLinks)
                .ThenInclude(bl => bl.Bottle)
            .FirstOrDefaultAsync(c => c.Id == capRequestId) ?? throw new ArgumentException("Cap edit request not found.");

        if (isEditConfirmed)
        {
            var currentCap = await context.Caps
                .Include(c => c.TextColorLinks)
                .Include(c => c.BackgroundColorLinks)
                .Include(c => c.BottleLinks)
                .FirstOrDefaultAsync(c => c.Id == currentCapId) ?? throw new ArgumentException("Current cap not found.");

            currentCap.TextOnCap = capRequest.TextOnCap;
            currentCap.Description = capRequest.Description;
            currentCap.CapPicture = capRequest.CapPicture;

            currentCap.TextColorLinks.Clear();
            currentCap.TextColorLinks = capRequest.TextColorLinks
                .Select(textColorLink => new CapToTextColor { CapId = currentCap.Id, Cap = currentCap, TextColorId = textColorLink.TextColorId, TextColor = textColorLink.TextColor })
                .ToList();

            currentCap.BackgroundColorLinks = capRequest.BackgroundColorLinks
                .Select(backgroundColorLink => new CapToBackgroundColor { CapId = currentCap.Id, Cap = currentCap, BackgroundColorId = backgroundColorLink.BackgroundColorId, BackgroundColor = backgroundColorLink.BackgroundColor })
                .ToList();

            currentCap.BottleLinks = capRequest.BottleLinks
                .Select(bottleLink => new CapToBottle { CapId = currentCap.Id, Cap = currentCap, BottleId = bottleLink.BottleId, Bottle = bottleLink.Bottle })
                .ToList();

            context.Caps.Update(currentCap);
        }

        context.Caps.Remove(capRequest);
        await context.SaveChangesAsync();
    }
    public async Task ConfirmBottleEdit(Guid currentBottleId, Guid bottleRequestId, bool isEditConfirmed)
    {
        var bottleRequest = await context.Bottles
            .Include(b => b.CapLinks)
            .ThenInclude(cl => cl.Cap)
            .FirstOrDefaultAsync(b => b.Id == bottleRequestId) ?? throw new ArgumentException("Bottle edit request not found.");

        if (isEditConfirmed)
        {
            var currentBottle = await context.Bottles
                .Include(b => b.CapLinks)
                .FirstOrDefaultAsync(b => b.Id == currentBottleId) ?? throw new ArgumentException("Current bottle not found.");

            currentBottle.Name = bottleRequest.Name;
            currentBottle.Description = bottleRequest.Description;
            currentBottle.Voltage = bottleRequest.Voltage;
            currentBottle.BottlePicture = bottleRequest.BottlePicture;
            currentBottle.DrinkType = bottleRequest.DrinkType;
            currentBottle.ProducerId = bottleRequest.ProducerId;
            currentBottle.CapLinks.Clear();
            foreach (var capLink in bottleRequest.CapLinks)
            {
                currentBottle.CapLinks.Add(new CapToBottle { CapId = capLink.CapId, Cap = capLink.Cap, BottleId = currentBottle.Id, Bottle = currentBottle });
            }

            context.Bottles.Update(currentBottle);
        }

        context.Bottles.Remove(bottleRequest);
        await context.SaveChangesAsync();
    }

    public async Task ConfirmProducerEdit(Guid currentProducerId, Guid producerRequestId, bool isEditConfirmed)
    {
        var producerRequest = await context.Producers.FindAsync(producerRequestId) ?? throw new ArgumentException("Producer edit request not found.");

        if (isEditConfirmed)
        {
            var currentProducer = await context.Producers.FindAsync(currentProducerId) ?? throw new ArgumentException("Current producer not found.");

            currentProducer.Name = producerRequest.Name;
            currentProducer.City = producerRequest.City;
            currentProducer.Description = producerRequest.Description;
            currentProducer.CountryId = producerRequest.CountryId;
        }

        context.Producers.Remove(producerRequest);
        await context.SaveChangesAsync();
    }
}
