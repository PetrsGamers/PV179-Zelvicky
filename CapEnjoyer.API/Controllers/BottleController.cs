namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BottleController(IBottleService bottleService, IImageService imageService) : ControllerBase
{
    [HttpPost("/upload-image/{bottleId:guid}")]
    public async Task<IActionResult> UploadImageForBottle(Guid bottleId, IFormFile image)
    {
        await imageService.UploadImageForBottleAsync(bottleId, image);
        return Ok("Image uploaded successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBottles()
    {
        var bottles = await bottleService.GetAllBottles();
        return Ok(bottles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBottleById(Guid id)
    {
        var bottle = await bottleService.GetBottleById(id);
        return Ok(bottle);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBottle([FromBody] BottleDto bottleDto)
    {
        var createdBottle = await bottleService.CreateBottle(bottleDto);
        return CreatedAtAction(nameof(GetBottleById), new { id = createdBottle.Id }, createdBottle);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBottle(Guid id, [FromBody] BottleDto bottle)
    {
        var updatedBottle = await bottleService.UpdateBottle(id, bottle);
        return Ok(updatedBottle);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBottle(Guid id)
    {
        await bottleService.DeleteBottle(id);
        return Ok();
    }
}
