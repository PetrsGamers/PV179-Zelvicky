namespace CapEnjoyer.BL.Services;

using Constants;
using DAL;
using DAL.Constants;
using DAL.Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class ImageService(CapEnjoyerDbContext context) : IImageService
{
    public async Task<string> UploadImageForCapAsync(Guid capId, IFormFile image)
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

        var cap = await context.Caps.FirstOrDefaultAsync(c => c.Id == capId) ??
                  throw new ArgumentException($"Cap with ID {capId} not found.");

        var sharedPath = Path.Combine("..", "SharedImages");

        Directory.CreateDirectory(sharedPath); // Ensure the directory exists

        var extension = Path.GetExtension(image.FileName);
        var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_cap{extension}";
        var filePath = Path.Combine(sharedPath, fileName);

        if (!cap.CapPicture.IsNullOrEmpty())
        {
            var oldFilePath = Path.Combine(sharedPath, cap.CapPicture);
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }
        }

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        cap.CapPicture = fileName;

        context.Caps.Update(cap);
        await context.AuditLogs.AddAsync(new AuditLog
        {
            Action = AuditLogAction.ImageUpload,
            EditedAt = DateTime.Now.ToUniversalTime(),
            Log = $"Image uploaded for cap with ID {capId}.",
            CapId = capId
        });
        await context.SaveChangesAsync();
        return fileName;
    }

    public async Task<string> UploadImageForBottleAsync(Guid bottleId, IFormFile image)
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

        var bottle = await context.Bottles.FirstOrDefaultAsync(b => b.Id == bottleId) ??
                     throw new ArgumentException($"Bottle with ID {bottleId} not found.");

        var sharedPath = Path.Combine("..", "SharedImages");

        Directory.CreateDirectory(sharedPath); // Ensure the directory exists

        var extension = Path.GetExtension(image.FileName);
        var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_bottle{extension}";
        var filePath = Path.Combine(sharedPath, fileName);

        if (!bottle.BottlePicture.IsNullOrEmpty())
        {
            var oldFilePath = Path.Combine(sharedPath, bottle.BottlePicture);
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }
        }

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        bottle.BottlePicture = fileName;

        context.Bottles.Update(bottle);
        await context.SaveChangesAsync();
        return fileName;
    }
}
