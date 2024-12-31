namespace Cap.Enjoyer.WebMVC.Models;

public class LeaderboardViewModel
{
    public required int Rank { get; set; }
    public required string Username { get; set; }
    public required int DistinctCapCount { get; set; }
    public bool IsPremium { get; set; }
}
