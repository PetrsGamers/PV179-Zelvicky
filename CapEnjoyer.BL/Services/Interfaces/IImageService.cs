namespace CapEnjoyer.BL.Services.Interfaces;

public interface IImageService
{
    public Task<string> UploadImageForCapAsync(Guid capId, IFormFile image);
    public Task<string> UploadImageForBottleAsync(Guid bottleId, IFormFile image);
}
