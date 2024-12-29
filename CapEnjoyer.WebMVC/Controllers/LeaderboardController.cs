namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

public class LeaderboardController(ILeaderboardService leaderboardService, IUserService userService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var leaderboard = await leaderboardService.GetLeaderboard();

        var viewModel = new List<LeaderboardViewModel>();
        foreach (var l in leaderboard)
        {
            var isPremium = await userService.IsPremiumUserAsync(l.Username);
            viewModel.Add(new LeaderboardViewModel
            {
                Rank = l.Rank,
                Username = l.Username,
                DistinctCapCount = l.DistinctCapCount,
                IsPremium = isPremium
            });
        }

        return View(viewModel);
    }
}
