namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Constants;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class CapSeed
{
    private const string CapSeedString = "default_cap_seed";

    private static readonly List<string> AestheticProperties =
    [
        "Colorful", "Glossy", "Sleek", "Sparkling", "Textured", "Vibrant", "Elegant", "Modern", "Retro", "Artistic",
        "Minimalistic", "Shiny", "Festive", "Chic", "Rustic", "Unique", "Classic", "Fancy", "Bold", "Whimsical"
    ];
    private static readonly List<string> FunctionalProperties =
    [
        "Sturdy", "Durable", "Lightweight", "Versatile", "Custom", "Secure", "Innovative", "Practical", "Premium",
        "Reliable", "Eco-friendly", "Functional", "Heat-resistant", "Waterproof", "Leak-proof", "Tamper-evident",
        "Insulated", "Safe", "Convenient", "Flexible"
    ];

    private static readonly List<string> CapSynonyms =
        [
            "cap",
            "lid",
            "seal",
            "stopper",
            "top",
            "crown seal",
            "crown cap",
            "crown cork",
        ];



    public static List<Cap> Seed(ModelBuilder modelBuilder, List<Color> colors, List<Album> albums,
        List<Bottle> bottles)
    {
        var caps = new List<Cap>();
        Randomizer.Seed = SeedUtils.GetRandom(CapSeedString);
        var capFaker = new Faker<Cap>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.TextOnCap, f => f.PickRandom(AestheticProperties) + " " + f.PickRandom(FunctionalProperties) + " " + f.PickRandom(CapSynonyms))
            .RuleFor(c => c.CapPicture, f => "default_cap_picture_url.jpg");

        for (var i = 0; i < 150; i++)
        {
            var cap = capFaker.Generate();
            cap.Description = $"This is a unique cap named '{cap.TextOnCap}'.";

            if (caps.Any(c => c.TextOnCap == cap.TextOnCap))
            {
                i--;
                continue;
            }
            caps.Add(cap);
        }
        modelBuilder.Entity<Cap>().HasData(caps);


        var faker = new Faker();
        foreach (var cap in caps)
        {
            var chosenBottles = faker.Random.ListItems(bottles, faker.Random.Number(1, 3)).ToList();
            AddBottlesToCap(modelBuilder, chosenBottles, cap);

            var chosenAlbums = faker.Random.ListItems(albums, faker.Random.Number(0, albums.Count / 2)).ToList();
            AddAlbumsToCap(modelBuilder, chosenAlbums, cap);

            var chosenTextColors = faker.Random.ListItems(colors, faker.Random.Number(1, 2)).ToList();
            AddTextColorsToCap(modelBuilder, chosenTextColors, cap);

            var chosenBackgroundColors = faker.Random.ListItems(colors, faker.Random.Number(1, 3)).ToList();
            AddBackgroundColorsToCap(modelBuilder, chosenBackgroundColors, cap);
        }

        return caps;
    }

    private static void AddAlbumsToCap(ModelBuilder modelBuilder, List<Album> albums, Cap cap)
    {
        foreach (var album in albums)
        {
            modelBuilder.Entity<Album>()
                .HasMany(a => a.Caps)
                .WithMany(c => c.Albums)
                .UsingEntity(j => j.HasData(new { AlbumsId = album.Id, CapsId = cap.Id }));
        }
    }

    private static void AddTextColorsToCap(ModelBuilder modelBuilder, List<Color> textColors, Cap cap)
    {
        foreach (var color in textColors)
        {
            modelBuilder.Entity<Cap>()
                .HasMany(c => c.TextColors)
                .WithMany(c => c.CapTexts)
                .UsingEntity(j => j.HasData(new { CapTextsId = cap.Id, TextColorsId = color.Id }));
        }
    }

    private static void AddBackgroundColorsToCap(ModelBuilder modelBuilder, List<Color> backgroundColors, Cap cap)
    {
        foreach (var color in backgroundColors)
        {
            modelBuilder.Entity<Cap>()
                .HasMany(c => c.BgColors)
                .WithMany(c => c.CapBackgrounds)
                .UsingEntity(j => j.HasData(new { CapBackgroundsId = cap.Id, BgColorsId = color.Id }));
        }
    }

    private static void AddBottlesToCap(ModelBuilder modelBuilder, List<Bottle> bottles, Cap cap)
    {
        foreach (var bottle in bottles)
        {
            modelBuilder.Entity<Cap>()
                .HasMany(c => c.Bottles)
                .WithMany(b => b.Caps)
                .UsingEntity(j => j.HasData(new { BottlesId = bottle.Id, CapsId = cap.Id }));
        }
    }
}
