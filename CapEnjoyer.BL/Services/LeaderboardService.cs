namespace CapEnjoyer.BL.Services;

using DAL;
using DTOs;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class LeaderboardService(CapEnjoyerDbContext context) : ILeaderboardService
{
    public async Task<List<LeaderboardDto>> GetLeaderboard()
    {
        var users = await context.Users
            .Include(u => u.Albums)
            .ThenInclude(a => a.CapLinks)
            .ThenInclude(ca => ca.Cap)
            .ToListAsync();

        var leaderboard = (from user in users
                let distinctCapIds = user.Albums.SelectMany(album => album.CapLinks)
                    .Select(capToAlbum => capToAlbum.Cap.Id)
                    .Distinct()
                    .ToList()
                select new LeaderboardDto
                {
                    DistinctCapCount = distinctCapIds.Count, Rank = 0, Username = user.Username
                })
            .ToList();

        leaderboard.Sort((x, y) => y.DistinctCapCount.CompareTo(x.DistinctCapCount));

        for (var i = 0; i < leaderboard.Count; i++)
        {
            leaderboard[i].Rank = i + 1;
        }

        return leaderboard;
    }
}
