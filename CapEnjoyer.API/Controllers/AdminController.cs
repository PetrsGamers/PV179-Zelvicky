namespace CapEnjoyer.API.Controllers;

using BL.DTOs;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IEditRequestService editRequestService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEditRequestInfo()
    {
        var result = await editRequestService.GetEditRequestInfo();
        return Ok(result);
    }

    [HttpPost("confirm-cap-edit")]
    public async Task<IActionResult> ConfirmCapEdit([FromBody] ConfirmEditRequestDto dto)
    {
        await editRequestService.ConfirmCapEdit(dto.CurrentId, dto.RequestId, dto.IsEditConfirmed);
        return Ok(dto.IsEditConfirmed ? "Cap edit confirmed successfully." : "Cap edit rejected successfully.");
    }

    [HttpPost("confirm-bottle-edit")]
    public async Task<IActionResult> ConfirmBottleEdit([FromBody] ConfirmEditRequestDto dto)
    {
        await editRequestService.ConfirmBottleEdit(dto.CurrentId, dto.RequestId, dto.IsEditConfirmed);
        return Ok(dto.IsEditConfirmed ? "Bottle edit confirmed successfully." : "Bottle edit rejected successfully.");
    }

    [HttpPost("confirm-producer-edit")]
    public async Task<IActionResult> ConfirmProducerEdit([FromBody] ConfirmEditRequestDto dto)
    {
        await editRequestService.ConfirmProducerEdit(dto.CurrentId, dto.RequestId, dto.IsEditConfirmed);
        return Ok(
            dto.IsEditConfirmed ? "Producer edit confirmed successfully." : "Producer edit rejected successfully.");
    }
}
