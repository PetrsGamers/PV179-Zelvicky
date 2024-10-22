namespace DAL.Seeds;

using Entities;
using Microsoft.EntityFrameworkCore;

public static class AlbumSeed
{
    public static List<Album> Seed(ModelBuilder modelBuilder, List<User> users)
    {
        var bivadId = users.Find(user => user.Username == "bivaD")?.Id ?? throw new Exception("User 'bivaD' not found.");
        var goretexakId = users.Find(user => user.Username == "goretexák")?.Id ?? throw new Exception("User 'bivvaD' not found.");
        var tedId = users.Find(user => user.Username == "ucitelkaLover69")?.Id ?? throw new Exception("User 'bivvvaD' not found.");
        var petaId = users.Find(user => user.Username == "koviďák")?.Id ?? throw new Exception("User 'bivaD' vnot found.");
        var monkeId = users.Find(user => user.Username == "Igorko")?.Id ?? throw new Exception("User 'bivaD' vvnot found.");



        var albums = new List<Album>([
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Czech caps",
                Description = "A collection of czech beer caps.",
                Public = false,
                UserId = bivadId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Foreign caps",
                Description = "A collection of foreign caps.",
                Public = true,
                UserId = bivadId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Soft drinks",
                Description = "It's miracle, but sometimes i drink even some non-alcoholic drinks.",
                Public = true,
                UserId = bivadId
            },

            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Covid caps",
                Description = "Who can drink more types of bottled beers while covid than me?",
                Public = true,
                UserId = petaId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "I DONT DRINK BEER",
                Description = "Demonstrative protesting blank album.",
                Public = true,
                UserId = petaId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "TS",
                Description = "Bottle caps with naked pictures of Taylor Swift.",
                Public = false,
                UserId = petaId
            },

            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Norge caps",
                Description = "Private norge bottle cap collection.",
                Public = false,
                UserId = goretexakId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "GORE-TEX collection",
                Description = "Caps with waterproof goretex\u2122 membrate.",
                Public = true,
                UserId = goretexakId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Ove's album",
                Description = "My host's collection.",
                Public = false,
                UserId = goretexakId
            },

            new Album
            {
                Id = Guid.NewGuid(),
                Name = "ISIC tour album",
                Description = "Caps looted at ISIC tour event.",
                Public = false,
                UserId = tedId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Society album of the Friends of PDF MUNI",
                Description = "Everything that hes been drunk on teambuilding.",
                Public = true,
                UserId = tedId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Tea bottle caps",
                Description = "These don't exist, but my girlfriend wanted her own album.",
                Public = true,
                UserId = tedId
            },

            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Patagonia Beers",
                Description = "Private cap album from patagonia",
                Public = false,
                UserId = monkeId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Norge girlfriend collection",
                Description = "Secret collection of expensive beer cups to impress my future Norge wife.",
                Public = false,
                UserId = monkeId
            },
            new Album
            {
                Id = Guid.NewGuid(),
                Name = "Zo Žiliny",
                Description = "Collection of local breweries from Žilina.",
                Public = true,
                UserId = monkeId
            }
        ]);

        foreach (var album in albums)
        {
            modelBuilder.Entity<Album>().HasData(album);
        }

        return albums;
    }
}
