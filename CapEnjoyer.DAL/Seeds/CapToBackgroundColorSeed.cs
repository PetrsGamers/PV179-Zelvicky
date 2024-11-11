namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class CapToBackgroundColorSeed
{
    private const string CapToBackgroundColorSeedString = "default_cap_to_background_color_seed";

    public static List<CapToBackgroundColor> Seed(ModelBuilder modelBuilder, List<Cap> caps, List<Color> colors)
    {
        var capBackgroundColorData = new List<CapToBackgroundColor>();
        Randomizer.Seed = SeedUtils.GetRandom(CapToBackgroundColorSeedString);

        var faker = new Faker();
        var capToBackgroundColorFaker = new Faker<CapToBackgroundColor>()
            .RuleFor(cb => cb.BackgroundColorId, f => f.PickRandom(colors).Id);

        foreach (var cap in caps)
        {
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
        }

        modelBuilder.Entity<CapToBackgroundColor>().HasData(capBackgroundColorData);
        return capBackgroundColorData;
    }
}
