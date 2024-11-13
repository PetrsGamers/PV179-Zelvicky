namespace CapEnjoyer.BL.Controllers;

using DAL;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(CapEnjoyerDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLeaderboard()
    {
        var users = await context.Users
            .Include(u => u.Albums)
            .ThenInclude(a => a.CapLinks)
            .ThenInclude(ca => ca.Cap)
            .ToListAsync();

        var leaderboard = new List<LeaderboardDto>();

        foreach (var user in users)
        {
            var distinctCapIds = user.Albums
                .SelectMany(album => album.CapLinks)
                .Select(capToAlbum => capToAlbum.Cap.Id)
                .Distinct()
                .ToList();

            leaderboard.Add(
                new LeaderboardDto { DistinctCapCount = distinctCapIds.Count, Rank = 0, Username = user.Username }
            );
        }

        leaderboard.Sort((x, y) => y.DistinctCapCount.CompareTo(x.DistinctCapCount));

        for (var i = 0; i < leaderboard.Count; i++)
        {
            leaderboard[i].Rank = i + 1;
        }

        return this.Ok(leaderboard);
    }
}
