namespace DAL.Seeds;

using Entities;
using Microsoft.EntityFrameworkCore;

public static class ProducerSeed
{
    public static List<Producer> Seed(ModelBuilder modelBuilder, List<Country> countries)
    {
        var czechCountry = countries.Find(country => country.Name == "Czech Republic");
        var usaCountry = countries.Find(country => country.Name == "United States");

        if (czechCountry == null || usaCountry == null)
        {
            throw new InvalidOperationException("Country not found.");
        }
        var czechId = czechCountry.Id;
        var usaId = usaCountry.Id;

        var producers = new List<Producer>
        {
            new() { Id = Guid.NewGuid(), Name = "Gambrinus Brewery", City = "Plzeň", Description = "Famous for its lager beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Pilsner Urquell", City = "Plzeň", Description = "World-renowned for its pale lager.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Ostrava Brewery", City = "Ostrava", Description = "Known for various types of beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Staropramen Brewery", City = "Prague", Description = "One of the largest breweries in the Czech Republic.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Velké Popovice Brewery", City = "Velké Popovice", Description = "Produces Kozel beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Krušovice Brewery", City = "Krušovice", Description = "Known for its dark beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Lobkowicz Brewery", City = "Vysoké Chvojno", Description = "Offers a variety of beers.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Humpolec Brewery (Bernard)", City = "Humpolec", Description = "Family-owned brewery with a rich history.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Nošovice Brewery (Radegast)", City = "Nošovice", Description = "Famous for its Radegast beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Svijany Brewery", City = "Svijany", Description = "Known for its traditional brewing methods.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Přerov Brewery (Zubr)", City = "Přerov", Description = "Produces Zubr beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Brno Brewery (Starobrno)", City = "Brno", Description = "Famous for its lager.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Hanušovice Brewery (Holba)", City = "Hanušovice", Description = "Known for its Holba beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Litovel Brewery", City = "Litovel", Description = "Offers a variety of traditional Czech beers.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Strakonice Brewery (Dudák)", City = "Strakonice", Description = "Known for its Dudák beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Březňák Brewery", City = "Březno", Description = "Part of Heineken group.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Benešov Brewery (Ferdinand)", City = "Benešov", Description = "Offers Ferdinand beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Dvůr Králové n. Labem Brewery (Tambor)", City = "Dvůr Králové", Description = "Known for its Tambor beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Chodová Planá Brewery", City = "Chodová Planá", Description = "Famous for its Chodovar beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Budweiser Brewery", City = "České Budějovice", Description = "Famous for its Budweiser beer.", CountryId = czechId},
            new() { Id = Guid.NewGuid(), Name = "Fanta", City = "Atlanta", Description = "A fruit-flavored carbonated soft drink.", CountryId = usaId },
            new() { Id = Guid.NewGuid(), Name = "Pepsi", City = "Purchase, New York", Description = "A major competitor to Coca-Cola.", CountryId = usaId },
            new() { Id = Guid.NewGuid(), Name = "Kofola", City = "Ostrava", Description = "A popular Czech soft drink.", CountryId = czechId },
            new() { Id = Guid.NewGuid(), Name = "Kingswood Cider", City = "Herefordshire", Description = "A refreshing cider made from apples.", CountryId = czechId }
        };

        foreach (var producer in producers)
        {
            modelBuilder.Entity<Producer>().HasData(producer);
        }

        return producers;
    }
}
