namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Constants;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class BottleSeed
{
    private const string BottleSeedString = "default_bottle_seed";

    private static readonly List<string> FirstProperties =
    [
        "Small", "Large", "Beautiful", "Elegant", "Fancy", "Rustic", "Modern", "Antique", "Vibrant", "Classic", "Dark",
        "Light", "Sleek", "Bold", "Quaint", "Exotic", "Unique", "Delicate", "Sturdy", "Shiny", "Charming", "Glamorous",
        "Sapphire", "Emerald", "Crystal", "Amber", "Ceramic", "Glass", "Wooden"
    ];

    private static readonly List<string> SecondProperties =
    [
        "Baroque", "Pirate", "Vintage", "Royal", "Mystic", "Funky", "Elegant", "Bold", "Chic", "Rustic", "Traditional",
        "Artisan", "Cultural", "Futuristic", "Tropical", "Gothic", "Cosmic", "Zen", "Urban", "Majestic", "Serene",
        "Passionate", "Epic", "Legendary", "Whimsical", "Enchanting", "Daring", "Nautical", "Rugged", "Sophisticated"
    ];

    public static List<Bottle> Seed(ModelBuilder modelBuilder, List<Producer> producers)
    {
        var bottles = new List<Bottle>();

        Randomizer.Seed = SeedUtils.GetRandom(BottleSeedString);
        var bottleFaker = new Faker<Bottle>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Voltage, f => Math.Round(f.Random.Float(3, 8), 2))
            .RuleFor(b => b.Name, f => f.PickRandom(FirstProperties) + " " + f.PickRandom(SecondProperties) + " bottle")
            .RuleFor(b => b.BottlePicture, f => "default_picture_url.jpg")
            .RuleFor(b => b.Description, (f, b) => $"A {b.Name} for various beverages.")
            .RuleFor(b => b.DrinkType, (f) => f.PickRandom<DrinkType>())
            .RuleFor(b => b.ProducerId, (f) => f.PickRandom(producers).Id);


        for (var i = 0; i < 150; i++)
        {
            var bottle = bottleFaker.Generate();
            if (bottles.Any(b => b.Name == bottle.Name))
            {
                i--;
                continue;
            }
            bottles.Add(bottle);
        }

        modelBuilder.Entity<Bottle>().HasData(bottles);
        return bottles;
    }
}
