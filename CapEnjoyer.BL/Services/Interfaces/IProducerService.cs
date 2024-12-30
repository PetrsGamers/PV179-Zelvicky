namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

public interface IProducerService
{
    Task<IEnumerable<ProducerDto>> GetAllProducersAsync();
    public Task<ProducerWithDetailsDto?> FindProducerWithDetailsByIdAsync(Guid id);
    Task<ProducerDto> GetProducerByIdAsync(Guid id);
    Task DeleteProducerAsync(Guid id);
    Task<ProducerDto> CreateProducerAsync(ProducerInsertDto producerDto);
    Task<ProducerDto> UpdateProducerAsync(Guid id, ProducerInsertDto producerDto);
    Task<IEnumerable<ProducerDto>> GetProducersBySearchFieldAsync(string searchField);
    Task<List<SelectListItem>> GetProducerOptionsAsync();
}
