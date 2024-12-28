namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProducerController(IProducerService producerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllProducers()
    {
        var producers = await producerService.GetAllProducersAsync();
        return Ok(producers);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProducerById(Guid id)
    {
        var producer = await producerService.GetProducerByIdAsync(id);
        return Ok(producer);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProducer(Guid id)
    {
        await producerService.DeleteProducerAsync(id);
        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> CreateProducer([FromBody] ProducerInsertDto producerDto)
    {
        var producer = await producerService.CreateProducerAsync(producerDto);
        return Ok(producer);
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProducer(Guid id, [FromBody] ProducerInsertDto producerDto)
    {
        var producer = await producerService.UpdateProducerAsync(id, producerDto);
        return Ok(producer);
    }
}
