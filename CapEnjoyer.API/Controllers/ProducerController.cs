namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProducerController(IProducerServiceAsync producerServiceAsync) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducers()
    {
        try
        {
            var producers = await producerServiceAsync.GetAllProducersAsync();
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
            var producer = await producerServiceAsync.GetProducerByIdAsync(id);
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
            await producerServiceAsync.DeleteProducerAsync(id);
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
            var producer = await producerServiceAsync.CreateProducerAsync(producerDto);
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
            var producer = await producerServiceAsync.UpdateProducerAsync(id, producerDto);
            return Ok(producer);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
