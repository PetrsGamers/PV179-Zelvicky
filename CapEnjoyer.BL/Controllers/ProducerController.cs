namespace CapEnjoyer.BL.Controllers;

using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ProducerController(IProducerService producerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducers()
    {
        try
        {
            var producers = await producerService.GetAllProducersAsync();
            return Ok(producers);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProducerById(Guid id)
    {
        try
        {
            var producer = await producerService.GetProducerByIdAsync(id);
            return Ok(producer);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProducer(Guid id)
    {
        try
        {
            await producerService.DeleteProducerAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateProducer([FromBody] ProducerInsertDto producerDto)
    {
        try
        {
            var producer = await producerService.CreateProducerAsync(producerDto);
            return Ok(producer);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProducer(Guid id, [FromBody] ProducerInsertDto producerDto)
    {
        try
        {
            var producer = await producerService.UpdateProducerAsync(id, producerDto);
            return Ok(producer);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
