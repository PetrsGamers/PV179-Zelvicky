namespace DAL.Seeds;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

public static class CapSeed
{
    private static readonly Random Random = new();

    public static List<Cap> Seed(ModelBuilder modelBuilder, List<Color> colors, List<Album> albums,
        List<Bottle> bottles)
    {
        var caps = new List<Cap>();
        var uniqueNames = new HashSet<string>();

        for (var i = 0; i < 150; i++)
        {
            var capName = GenerateUniqueCapName(uniqueNames);


            var cap = new Cap
            {
                Id = Guid.NewGuid(),
                TextOnCap = capName,
                Description = $"This is a unique cap named '{capName}'.",
                CapPicture = "default_cap_picture_url.jpg",
            };
            caps.Add(cap);

        }
        foreach (var cap in caps)
        {
            modelBuilder.Entity<Cap>().HasData(cap);

            List<Bottle> chosenBottles = [bottles[Random.Next(bottles.Count)]];
            AddBottlesToCap(modelBuilder, chosenBottles, cap);

            var chosenAlbums =
                albums.OrderBy(a => Random.Next()).Take(Random.Next(0, albums.Count + 1)).ToList();
            AddAlbumsToCap(modelBuilder, chosenAlbums, cap);

            var chosenTextColors = GetRandomColors(colors, 1, 2);
            AddTextColorsToCap(modelBuilder, chosenTextColors, cap);

            var chosenBackgroundColors = GetRandomColors(colors, 1, 3);
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
                .UsingEntity(j => j.HasData(new
                {
                    AlbumsId = album.Id,
                    CapsId = cap.Id
                }));
        }    }

    private static void AddTextColorsToCap(ModelBuilder modelBuilder, List<Color> textColors, Cap cap)
    {
        foreach (var color in textColors)
        {
            modelBuilder.Entity<Cap>()
                .HasMany(c => c.TextColors)
                .WithMany(c => c.CapTexts)
                .UsingEntity(j => j.HasData(new
                {
                    CapTextsId = cap.Id,
                    TextColorsId = color.Id
                }));
        }
    }

    private static void AddBackgroundColorsToCap(ModelBuilder modelBuilder, List<Color> backgroundColors, Cap cap)
    {
        foreach (var color in backgroundColors)
        {
            modelBuilder.Entity<Cap>()
                .HasMany(c => c.BgColors)
                .WithMany(c => c.CapBackgrounds)
                .UsingEntity(j => j.HasData(new
                {
                    CapBackgroundsId = cap.Id,
                    BgColorsId = color.Id
                }));
        }    }

    private static void AddBottlesToCap(ModelBuilder modelBuilder, List<Bottle> bottles, Cap cap)
    {
        foreach (var bottle in bottles)
        {
            modelBuilder.Entity<Cap>()
                .HasMany(c => c.Bottles)
                .WithMany(b => b.Caps)
                .UsingEntity(j => j.HasData(new
                {
                    BottlesId = bottle.Id,
                    CapsId = cap.Id
                }));
        }
    }

    private static List<Color> GetRandomColors(List<Color> colors, int min, int max)
    {
        if (min <= 0 || min > max)
        {
            throw new ArgumentOutOfRangeException(nameof(min), "Minimum must be greater than zero and less than or equal to maximum.");
        }
        max = Math.Min(max, colors.Count);
        var count = Random.Next(min, max + 1);

        var shuffledColors = colors.OrderBy(c => Random.Next()).ToList();

        return shuffledColors.Take(count).ToList();
    }

    private static string GenerateUniqueCapName(HashSet<string> uniqueNames)
    {
        string[] aestheticAdjectives = ["Colorful", "Glossy", "Sleek", "Sparkling", "Textured", "Vibrant", "Elegant", "Modern", "Retro", "Artistic", "Minimalistic", "Shiny", "Festive", "Chic", "Rustic", "Unique", "Classic", "Fancy", "Bold", "Whimsical"];
        string[] functionalAdjectives = ["Sturdy", "Durable", "Lightweight", "Versatile", "Custom", "Secure", "Innovative", "Practical", "Premium", "Reliable", "Eco-friendly", "Functional", "Heat-resistant", "Waterproof", "Leak-proof", "Tamper-evident", "Insulated", "Safe", "Convenient", "Flexible"];

        string name;
        do
        {
            var aestheticAdjective = aestheticAdjectives[Random.Next(aestheticAdjectives.Length)];
            var functionalAdjective = functionalAdjectives[Random.Next(functionalAdjectives.Length)];
            name = $"{aestheticAdjective} {functionalAdjective} cap";
        }
        while (uniqueNames.Contains(name));

        uniqueNames.Add(name);
        return name;
    }


}
