namespace CapEnjoyer.BL.Interfaces;

using DTOs;

public interface IBottleService
{
    Task UploadImageForBottleAsync(Guid bottleId, IFormFile image);
    Task<BottleDto> GetBottleById(Guid id);
    Task<IEnumerable<BottleDto>> GetAllBottles();
    Task<BottleDto> CreateBottle(BottleDto bottle);
    Task<BottleDto> UpdateBottle(Guid id, BottleDto bottle);
    Task DeleteBottle(Guid id);
}
