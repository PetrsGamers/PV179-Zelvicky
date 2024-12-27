namespace Cap.Enjoyer.WebMVC.Controllers;
using Cap.Enjoyer.WebMVC.Models;
using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class LeaderboardController(ILeaderboardService leaderboardService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var leaderboard = await leaderboardService.GetLeaderboard();
        var viewModel = leaderboard.Select(l => new LeaderboardViewModel
        {
            Rank = l.Rank,
            Username = l.Username,
            DistinctCapCount = l.DistinctCapCount
        });
        return View(viewModel);
    }
}
