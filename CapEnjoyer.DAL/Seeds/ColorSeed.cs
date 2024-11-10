namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class ColorSeed
{
    private const string ColorsSeedString = "basic_color_seed";

    private static readonly List<(string Name, string HexCode)> DefaultColors =
    [
        ("Black", "#141519"),
        ("Red", "#a02722"),
        ("Orange", "#f07613"),
        ("Green", "#546d1b"),
        ("Brown", "#724728"),
        ("Blue", "#35399d"),
        ("Purple", "#792aac"),
        ("Cyan", "#158991"),
        ("Light Gray", "#8e8e86"),
        ("Light Blue", "#3aafd9"),
        ("Gray", "#3e4447"),
        ("Pink", "#ed8dac"),
        ("Lime", "#70b919"),
        ("Yellow", "#f8c527"),
        ("White", "#e9ecec"),
        ("Magenta", "#bd44b3")
    ];

    public static List<Color> Seed(ModelBuilder modelBuilder)
    {
        Randomizer.Seed = SeedUtils.GetRandom(ColorsSeedString);

        var colorFaker = new Faker<Color>()
            .RuleFor(c => c.Id, f => f.Random.Guid());

        List<Color> colors = new();
        foreach (var (name, hexCode) in DefaultColors)
        {
            var color = colorFaker.Generate();
            color.Name = name;
            color.HexCode = hexCode;
            colors.Add(color);
        }

        modelBuilder.Entity<Color>().HasData(colors);
        return colors;
    }
}
