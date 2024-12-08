namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Constants;
using DAL.Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class ImageService(CapEnjoyerDbContext context) : IImageService
{
    public async Task UploadImageForCapAsync(Guid capId, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Invalid file.");
        }

        if (image.Length > ImageConstants.MaxImageSize)
        {
            throw new ArgumentException("File size is too big.");
        }

        if (!ImageConstants.AllowedContentTypes.Contains(image.ContentType))
        {
            throw new ArgumentException("Invalid file type.");
        }

        var baseDirectory = Directory.GetCurrentDirectory();
        var uploadsFolder = Path.Combine(baseDirectory, ImageConstants.CapImageFolder);
        Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

        var fileName = $"{Guid.NewGuid()}_cap_{image.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var cap = await context.Caps.FirstOrDefaultAsync(c => c.Id == capId) ??
                  throw new ArgumentException($"Cap with ID {capId} not found.");
        cap.CapPicture = filePath;

        context.Caps.Update(cap);
        await context.AuditLogs.AddAsync(new AuditLog
        {
            Action = AuditLogAction.ImageUpload,
            EditedAt = DateTime.Now.ToUniversalTime(),
            Log = $"Image uploaded for cap with ID {capId}.",
            CapId = capId
        });
        await context.SaveChangesAsync();
    }

    public async Task UploadImageForBottleAsync(Guid bottleId, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Invalid file.");
        }

        if (image.Length > ImageConstants.MaxImageSize)
        {
            throw new ArgumentException("File size is too big.");
        }

        if (!ImageConstants.AllowedContentTypes.Contains(image.ContentType))
        {
            throw new ArgumentException("Invalid file type.");
        }

        var baseDirectory = Directory.GetCurrentDirectory();
        var uploadsFolder = Path.Combine(baseDirectory, ImageConstants.BottleImageFolder);
        Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

        var fileName = $"{Guid.NewGuid()}_bottle_{image.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var bottle = await context.Bottles.FirstOrDefaultAsync(b => b.Id == bottleId) ??
                     throw new ArgumentException($"Bottle with ID {bottleId} not found.");
        bottle.BottlePicture = filePath;

        context.Bottles.Update(bottle);
        await context.SaveChangesAsync();
    }
}
