namespace CapEnjoyer.BL.Services.Interfaces;

using DTOs;

public interface ILeaderboardService
{
    public Task<List<LeaderboardDto>> GetLeaderboard();
}
