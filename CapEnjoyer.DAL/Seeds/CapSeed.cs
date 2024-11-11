namespace CapEnjoyer.DAL.Seeds;

using System.Globalization;
using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class CapSeed
{
    private const string CapSeedString = "default_cap_seed";
    private const string DefaultCapPicture = "default_cap_picture_url.jpg";
    private const string DescriptionTemplate = "This is a unique cap named '{0}'.";

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


    public static List<Cap> Seed(ModelBuilder modelBuilder)
    {
        var caps = new List<Cap>();
        Randomizer.Seed = SeedUtils.GetRandom(CapSeedString);
        var capFaker = new Faker<Cap>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.TextOnCap,
                f => f.PickRandom(AestheticProperties) + " " + f.PickRandom(FunctionalProperties) + " " +
                     f.PickRandom(CapSynonyms))
            .RuleFor(c => c.CapPicture, _ => DefaultCapPicture)
            .RuleFor(c => c.Description,
                (_, u) => string.Format(CultureInfo.InvariantCulture, DescriptionTemplate, u.TextOnCap));

        for (var i = 0; i < 150; i++)
        {
            var cap = capFaker.Generate();

            if (caps.Any(c => c.TextOnCap == cap.TextOnCap))
            {
                i--;
                continue;
            }

            caps.Add(cap);
        }

        for (var i = 10; i < 17; i++)
        {
            caps[i].Description = $"Updated description {i}";
            caps[i].IsEditForId = caps[1].Id;
        }

        modelBuilder.Entity<Cap>().HasData(caps);
        return caps;
    }
}
