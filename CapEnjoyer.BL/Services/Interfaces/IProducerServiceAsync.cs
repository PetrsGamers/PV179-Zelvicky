namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface IProducerServiceAsync
{
    Task<IEnumerable<ProducerDto>> GetAllProducersAsync();
    Task<ProducerDto> GetProducerByIdAsync(Guid id);
    Task DeleteProducerAsync(Guid id);
    Task<ProducerDto> CreateProducerAsync(ProducerInsertDto producerDto);
    Task<ProducerDto> UpdateProducerAsync(Guid id, ProducerInsertDto producerDto);
}
