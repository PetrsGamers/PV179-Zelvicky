using Microsoft.AspNetCore.Mvc;
using DAL.BL.DTOs;

namespace DAL.BL.Controllers;

using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(ApplicationContext context, ILogger<LeaderboardController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetLeaderboard()
    {
        try
        {
            var userIds = context.Users.Select(u => u.Id).ToList();
            List<LeaderboardDto> leaderboard = [];
            foreach (var userId in userIds)
            {
                var user = context.Users
                    .Include(u => u.Albums)
                    .ThenInclude(a => a.Caps)
                    .FirstOrDefault(u => u.Id == userId);

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
