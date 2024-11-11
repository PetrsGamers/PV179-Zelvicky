namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class CapToBottleSeed
{
    private const string CapToBottleSeedString = "default_cap_to_bottle_seed";

    public static List<CapToBottle> Seed(ModelBuilder modelBuilder, List<Cap> caps, List<Bottle> bottles)
    {
        var capBottleData = new List<CapToBottle>();
        Randomizer.Seed = SeedUtils.GetRandom(CapToBottleSeedString);

        var faker = new Faker();
        var capToBottleFaker = new Faker<CapToBottle>()
            .RuleFor(cb => cb.BottleId, f => f.PickRandom(bottles).Id);

        foreach (var cap in caps)
        {
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

        modelBuilder.Entity<CapToBottle>().HasData(capBottleData);
        return capBottleData;
    }
}
