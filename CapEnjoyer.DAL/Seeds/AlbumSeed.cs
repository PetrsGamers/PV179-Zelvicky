namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Constants;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class AlbumSeed
{
    private const string DavidAlbumSeedString = "david_album_seed";
    private const string SenatorAlbumSeedString = "senator_album_seed";
    private const string GoretexakAlbumSeedString = "goretexak_album_seed";
    private const string TedAlbumSeedString = "ted_album_seed";
    private const string MonkeSeedString = "monke_album_seed";

    private static readonly List<(string Name, string Description)> BivaDAlbums =
    [
        ("Czech caps", "A collection of czech beer caps."),
        ("Foreign caps", "A collection of foreign caps."),
        ("Soft drinks", "It's miracle, but sometimes i drink even some non-alcoholic drinks.")
    ];

    private static readonly List<(string Name, string Description)> SenatorAlbums =
    [
        ("I DONT DRINK BEER", "Demonstrative protesting blank album."),
        ("TS", "Bottle caps with naked pictures of Taylor Swift.")
    ];

    private static readonly List<(string Name, string Description)> GoretexakAlbums =
    [
        ("Norge caps", "Private norge bottle cap collection."),
        ("GORE-TEX collection", "Caps with waterproof goretex™ membrate."),
        ("Ove's album", "My host's collection.")
    ];

    private static readonly List<(string Name, string Description)> TedAlbums =
    [
        ("ISIC tour album", "Caps looted at ISIC tour event."),
        ("Society album of the Friends of PDF MUNI", "Everything that hes been drunk on teambuilding.")
    ];

    private static readonly List<(string Name, string Description)> MonkeAlbums =
    [
        ("Tea bottle caps", "These don't exist, but my girlfriend (which also dont exist wanted her own album."),
        ("Patagonia Beers", "Private cap album from patagonia"),
        ("Norge girlfriend collection", "Secret collection of expensive beer cups to impress my future Norge wife."),
        ("Zo Žiliny", "Collection of local breweries from Žilina.")
    ];

    private static readonly
        List<(string Username, string SeedString, List<(string Name, string Description)> AlbumInfos)> UsersAlbumData =
        [
            ("bivaD", DavidAlbumSeedString, BivaDAlbums),
            ("goretexak", GoretexakAlbumSeedString, GoretexakAlbums),
            ("Ted", TedAlbumSeedString, TedAlbums),
            ("Senator", SenatorAlbumSeedString, SenatorAlbums),
            ("Monke", MonkeSeedString, MonkeAlbums)
        ];

    public static List<Album> Seed(ModelBuilder modelBuilder, List<User> users)
    {
        users = users.Where(u => u.Role == Role.User).ToList();

        var albumFaker = new Faker<Album>()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.Public, f => f.Random.Bool());

        var albums = new List<Album>();

        foreach (var (username, seedString, albumInfos) in UsersAlbumData)
        {
            Randomizer.Seed = SeedUtils.GetRandom(seedString);
            var userId = SeedUtils.GetUserId(username, users);
            albums.AddRange(GenerateAlbumsForUser(userId, albumInfos, albumFaker));
        }

        modelBuilder.Entity<Album>().HasData(albums);
        return albums;
    }

    private static List<Album> GenerateAlbumsForUser(Guid userId, List<(string Name, string Description)> albumInfos,
        Faker<Album> albumFaker)
    {
        var generatedAlbums = new List<Album>();

        foreach (var (name, description) in albumInfos)
        {
            var album = albumFaker.Generate();
            album.Name = name;
            album.Description = description;
            album.UserId = userId;

            generatedAlbums.Add(album);
        }

        return generatedAlbums;
    }
}
