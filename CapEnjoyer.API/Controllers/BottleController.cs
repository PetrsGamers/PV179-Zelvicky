namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BottleController(IBottleService bottleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllBottles()
    {
        try
        {
            var bottles = await bottleService.GetAllBottles();
            return this.Ok(bottles);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBottleById(Guid id)
    {
        try
        {


            var bottle = await bottleService.GetBottleById(id);


            return this.Ok(bottle);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBottle([FromBody] BottleDto bottleDto)
    {
        try
        {
            var createdBottle = await bottleService.CreateBottle(bottleDto);


            return this.CreatedAtAction(nameof(this.GetBottleById), new { id = createdBottle.Id }, createdBottle);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBottle(Guid id, [FromBody] BottleDto bottle)
    {
        try
        {
            var updatedBottle = await bottleService.UpdateBottle(id, bottle);


            return this.Ok(updatedBottle);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBottle(Guid id)
    {
        try
        {
            await bottleService.DeleteBottle(id);

            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

}
