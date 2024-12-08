namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BottleController(IBottleService bottleService) : ControllerBase
{
    [HttpPost("/upload-image/{bottleId:guid}")]
    public async Task<IActionResult> UploadImageForBottle(Guid bottleId, IFormFile image)
    {
        try
        {
            await imageService.UploadImageForBottleAsync(bottleId, image);
            return Ok("Image uploaded successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        await bottleService.UploadImageForBottleAsync(bottleId, image);
        return Ok("Image uploaded successfully.");

    }

    [HttpGet]
    public async Task<IActionResult> GetAllBottles()
    {

        var bottles = await bottleService.GetAllBottles();
        return this.Ok(bottles);

    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBottleById(Guid id)
    {

        var bottle = await bottleService.GetBottleById(id);
        return this.Ok(bottle);

    }

    [HttpPost]
    public async Task<IActionResult> CreateBottle([FromBody] BottleDto bottleDto)
    {

        var createdBottle = await bottleService.CreateBottle(bottleDto);
        return this.CreatedAtAction(nameof(this.GetBottleById), new { id = createdBottle.Id }, createdBottle);

    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBottle(Guid id, [FromBody] BottleDto bottle)
    {

        var updatedBottle = await bottleService.UpdateBottle(id, bottle);
        return this.Ok(updatedBottle);

    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBottle(Guid id)
    {

        await bottleService.DeleteBottle(id);
        return this.Ok();

    }
}
