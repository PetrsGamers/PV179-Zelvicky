namespace CapEnjoyer.BL.Services.Interfaces;

public interface IImageService
{
    public Task UploadImageForCapAsync(Guid capId, IFormFile image);
    public Task UploadImageForBottleAsync(Guid bottleId, IFormFile image);
}
