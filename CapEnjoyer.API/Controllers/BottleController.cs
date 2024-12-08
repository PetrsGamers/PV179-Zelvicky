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
        try
        {
            await imageService.UploadImageForBottleAsync(bottleId, image);
            return Ok("Image uploaded successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBottles()
    {
        try
        {
            var bottles = await bottleService.GetAllBottles();
            return Ok(bottles);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBottleById(Guid id)
    {
        try
        {
            var bottle = await bottleService.GetBottleById(id);
            return Ok(bottle);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBottle([FromBody] BottleDto bottleDto)
    {
        try
        {
            var createdBottle = await bottleService.CreateBottle(bottleDto);
            return CreatedAtAction(nameof(GetBottleById), new { id = createdBottle.Id }, createdBottle);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBottle(Guid id, [FromBody] BottleDto bottle)
    {
        try
        {
            var updatedBottle = await bottleService.UpdateBottle(id, bottle);
            return Ok(updatedBottle);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBottle(Guid id)
    {
        try
        {
            await bottleService.DeleteBottle(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
