namespace CapEnjoyer.BL.Controllers;

using DAL;
using DAL.Entities;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(CapEnjoyerDbContext context, ILogger<LeaderboardController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLeaderboard()
    {
        try
        {
            var userIds = await context.Users.Select(u => u.Id).ToListAsync();
            List<LeaderboardDto> leaderboard = [];
            foreach (var userId in userIds)
            {
                var user = await context.Users
                    .Include(u => u.Albums)
                    .ThenInclude(a => a.Caps)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return this.NotFound("User not found.");
                }

                var caps = user.Albums
                    .SelectMany(album => album.Caps ?? Enumerable.Empty<Cap>())
                    .Select(cap => cap.Id)
                    .Distinct()
                    .ToList();

                leaderboard.Add(
                    new LeaderboardDto { DistinctCapCount = caps.Count, Rank = 0, Username = user.Username }
                );
            }

            leaderboard.Sort((x, y) => y.DistinctCapCount.CompareTo(x.DistinctCapCount));

            for (var i = 0; i < leaderboard.Count; i++)
            {
                leaderboard[i].Rank = i + 1;
            }

            return this.Ok(leaderboard);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }
}
