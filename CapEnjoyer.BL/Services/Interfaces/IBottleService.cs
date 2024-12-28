namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface IBottleService
{
    public Task<BottleDto> GetBottleById(Guid id);
    public Task<IEnumerable<BottleDto>> GetAllBottles();
    public Task<BottleDto> CreateBottle(BottleInsertDto bottle);
    public Task<BottleDto> UpdateBottle(Guid id, BottleInsertDto bottle);
    public Task DeleteBottle(Guid id);
}
