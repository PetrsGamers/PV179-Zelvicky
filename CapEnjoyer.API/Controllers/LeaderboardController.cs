namespace CapEnjoyer.API.Controllers;

using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(ILeaderboardService leaderboardService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLeaderboard()
    {
        var leaderboard = await leaderboardService.GetLeaderboard();
        return Ok(leaderboard);
    }
}
