namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface IBottleService
{
    public Task<BottleDto> GetBottleById(Guid id);
    public Task<IEnumerable<BottleDto>> GetAllBottles();
    public Task<BottleDto> CreateBottle(BottleDto bottle);
    public Task<BottleDto> UpdateBottle(Guid id, BottleDto bottle);
    public Task DeleteBottle(Guid id);
}
