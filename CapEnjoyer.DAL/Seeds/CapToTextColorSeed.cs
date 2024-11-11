namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class CapToTextColorSeed
{
    private const string CapToTextColorSeedString = "default_cap_to_text_color_seed";

    public static List<CapToTextColor> Seed(ModelBuilder modelBuilder, List<Cap> caps, List<Color> colors)
    {
        var capTextColorData = new List<CapToTextColor>();
        Randomizer.Seed = SeedUtils.GetRandom(CapToTextColorSeedString);

        var faker = new Faker();
        var capToTextColorFaker = new Faker<CapToTextColor>()
            .RuleFor(ct => ct.TextColorId, f => f.PickRandom(colors).Id);

        foreach (var cap in caps)
        {
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
        }

        modelBuilder.Entity<CapToTextColor>().HasData(capTextColorData);
        return capTextColorData;
    }
}
