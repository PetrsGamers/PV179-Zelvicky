namespace CapEnjoyer.DAL.Seeds;

using Bogus;
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
        "cap", "lid", "seal", "stopper", "top", "crown seal", "crown cap", "crown cork"
    ];


    public static List<Cap> Seed(ModelBuilder modelBuilder, List<Color> colors, List<Album> albums,
        List<Bottle> bottles)
    {
        var caps = new List<Cap>();
        var capTextColorData = new List<CapToTextColor>();
        var capBackgroundColorData = new List<CapToBackgroundColor>();
        var capAlbumData = new List<CapToAlbum>();
        var capBottleData = new List<CapToBottle>();

        Randomizer.Seed = SeedUtils.GetRandom(CapSeedString);
        var capFaker = new Faker<Cap>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.TextOnCap,
                f => f.PickRandom(AestheticProperties) + " " + f.PickRandom(FunctionalProperties) + " " +
                     f.PickRandom(CapSynonyms))
            .RuleFor(c => c.CapPicture, f => "default_cap_picture_url.jpg")
            .RuleFor(c => c.Description, (f, u) => $"This is a unique cap named '{u.TextOnCap}'.");

        var capToTextColorFaker = new Faker<CapToTextColor>()
            .RuleFor(ct => ct.TextColorId, f => f.PickRandom(colors).Id);

        var capToBackgroundColorFaker = new Faker<CapToBackgroundColor>()
            .RuleFor(cb => cb.BackgroundColorId, f => f.PickRandom(colors).Id);

        var capToAlbumFaker = new Faker<CapToAlbum>()
            .RuleFor(ca => ca.AlbumId, f => f.PickRandom(albums).Id);

        var capToBottleFaker = new Faker<CapToBottle>()
            .RuleFor(cb => cb.BottleId, f => f.PickRandom(bottles).Id);

        var faker = new Faker();
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

            var textColorsCount = faker.Random.Number(1, 2);
            for (var j = 0; j < textColorsCount; j++)
            {
                var capToTextColor = capToTextColorFaker.Generate();
                capToTextColor.CapId = cap.Id;

                if (capTextColorData.Any(c =>
                        c.TextColorId == capToTextColor.TextColorId && c.CapId == capToTextColor.CapId))
                {
                    j--;
                    continue;
                }

                capTextColorData.Add(capToTextColor);
            }

            var backgroundColorsCount = faker.Random.Number(1, 3);
            for (var k = 0; k < backgroundColorsCount; k++)
            {
                var capToBackgroundColor = capToBackgroundColorFaker.Generate();
                capToBackgroundColor.CapId = cap.Id;

                if (capBackgroundColorData.Any(c =>
                        c.BackgroundColorId == capToBackgroundColor.BackgroundColorId &&
                        c.CapId == capToBackgroundColor.CapId))
                {
                    k--;
                    continue;
                }

                capBackgroundColorData.Add(capToBackgroundColor);
            }

            var chosenAlbumsCount = faker.Random.Number(1, albums.Count / 2);
            for (var k = 0; k < chosenAlbumsCount; k++)
            {
                var capToAlbum = capToAlbumFaker.Generate();
                capToAlbum.CapId = cap.Id;

                if (capAlbumData.Any(c => c.AlbumId == capToAlbum.AlbumId && c.CapId == capToAlbum.CapId))
                {
                    k--;
                    continue;
                }

                capAlbumData.Add(capToAlbum);
            }

            var chosenBottlesCount = faker.Random.Number(0, 2);
            for (var k = 0; k < chosenBottlesCount; k++)
            {
                var capToBottle = capToBottleFaker.Generate();
                capToBottle.CapId = cap.Id;

                if (capBottleData.Any(c => c.BottleId == capToBottle.BottleId && c.CapId == capToBottle.CapId))
                {
                    k--;
                    continue;
                }

                capBottleData.Add(capToBottle);
            }
        }

        modelBuilder.Entity<Cap>().HasData(caps);
        modelBuilder.Entity<CapToTextColor>().HasData(capTextColorData);
        modelBuilder.Entity<CapToBackgroundColor>().HasData(capBackgroundColorData);
        modelBuilder.Entity<CapToAlbum>().HasData(capAlbumData);
        modelBuilder.Entity<CapToBottle>().HasData(capBottleData);

        return caps;
    }
}
