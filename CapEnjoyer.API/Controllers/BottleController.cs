namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BottleController(IBottleService bottleService) : ControllerBase
{
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
    public async Task<IActionResult> CreateBottle(BottleInsertDto bottleDto)
    {
        var createdBottle = await bottleService.CreateBottle(bottleDto);
        return Ok(createdBottle);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBottle(Guid id, BottleInsertDto bottle)
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
