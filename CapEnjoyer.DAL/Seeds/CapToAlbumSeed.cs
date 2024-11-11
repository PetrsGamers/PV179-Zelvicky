namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class CapToAlbumSeed
{
    private const string CapToAlbumSeedString = "default_cap_to_album_seed";

    public static List<CapToAlbum> Seed(ModelBuilder modelBuilder, List<Cap> caps, List<Album> albums)
    {
        var capAlbumData = new List<CapToAlbum>();
        Randomizer.Seed = SeedUtils.GetRandom(CapToAlbumSeedString);

        var faker = new Faker();
        var capToAlbumFaker = new Faker<CapToAlbum>()
            .RuleFor(ca => ca.AlbumId, f => f.PickRandom(albums).Id);

        foreach (var cap in caps)
        {
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
        }

        modelBuilder.Entity<CapToAlbum>().HasData(capAlbumData);
        return capAlbumData;
    }
}
