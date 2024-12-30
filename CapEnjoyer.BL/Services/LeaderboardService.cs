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

        var leaderboard = users
            .Select(user => new LeaderboardDto
            {
                Username = user.Username,
                DistinctCapCount = user.Albums
                    .SelectMany(album => album.CapLinks)
                    .Select(capToAlbum => capToAlbum.Cap?.Id)
                    .Distinct()
                    .Count(),
            })
            .OrderByDescending(dto => dto.DistinctCapCount)
            .Select((dto, index) =>
            {
                dto.Rank = index + 1;
                return dto;
            })
            .ToList();

        return leaderboard;
    }

}
