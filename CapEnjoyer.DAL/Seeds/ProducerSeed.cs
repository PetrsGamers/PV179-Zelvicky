namespace CapEnjoyer.DAL.Seeds;

using Bogus;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class ProducerSeed
{
    private const string CzechProducerSeedString = "czech_producer_seed";
    private const string UsaProducerSeedString = "usa_producer_seed";

    private static readonly List<(string Name, string City, string Description)> CzechProducers =
    [
        ("Gambrinus Brewery", "Plzeň", "Famous for its lager beer."),
        ("Pilsner Urquell", "Plzeň", "World-renowned for its pale lager."),
        ("Ostrava Brewery", "Ostrava", "Known for various types of beer."),
        ("Staropramen Brewery", "Prague", "One of the largest breweries in the Czech Republic."),
        ("Velké Popovice Brewery", "Velké Popovice", "Produces Kozel beer."),
        ("Krušovice Brewery", "Krušovice", "Known for its dark beer."),
        ("Lobkowicz Brewery", "Vysoké Chvojno", "Offers a variety of beers."),
        ("Humpolec Brewery (Bernard)", "Humpolec", "Family-owned brewery with a rich history."),
        ("Nošovice Brewery (Radegast)", "Nošovice", "Famous for its Radegast beer."),
        ("Svijany Brewery", "Svijany", "Known for its traditional brewing methods."),
        ("Přerov Brewery (Zubr)", "Přerov", "Produces Zubr beer."),
        ("Brno Brewery (Starobrno)", "Brno", "Famous for its lager."),
        ("Hanušovice Brewery (Holba)", "Hanušovice", "Known for its Holba beer."),
        ("Litovel Brewery", "Litovel", "Offers a variety of traditional Czech beers."),
        ("Strakonice Brewery (Dudák)", "Strakonice", "Known for its Dudák beer."),
        ("Březňák Brewery", "Březno", "Part of Heineken group."),
        ("Benešov Brewery (Ferdinand)", "Benešov", "Offers Ferdinand beer."),
        ("Dvůr Králové n. Labem Brewery (Tambor)", "Dvůr Králové", "Known for its Tambor beer."),
        ("Chodová Planá Brewery", "Chodová Planá", "Famous for its Chodovar beer."),
        ("Budweiser Brewery", "České Budějovice", "Famous for its Budweiser beer."),
        ("Kofola", "Ostrava", "A popular Czech soft drink."),
        ("Kingswood Cider", "Herefordshire", "A refreshing cider made from apples.")
    ];

    private static readonly List<(string Name, string City, string Description)> UsaProducers =
    [
        ("Fanta", "Atlanta", "A fruit-flavored carbonated soft drink."),
        ("Pepsi", "Purchase, New York", "A major competitor to Coca-Cola.")
    ];

    private static readonly
        List<(string SeedString, string CountryName, List<(string Name, string City, string Description)> Producers)>
        ProducerData =
        [
            (CzechProducerSeedString, "Czech Republic", CzechProducers),
            (UsaProducerSeedString, "United States", UsaProducers)
        ];

    public static List<Producer> Seed(ModelBuilder modelBuilder, List<Country> countries)
    {
        var producers = new List<Producer>();
        var producerFaker = new Faker<Producer>().RuleFor(c => c.Id, f => f.Random.Guid());

        foreach (var (seedString, countryName, producerInfo) in ProducerData)
        {
            var country = countries.Find(c => c.Name == countryName);
            if (country == null)
            {
                throw new InvalidOperationException($"Country '{countryName}' not found.");
            }

            producers.AddRange(GenerateProducersForCountry(seedString, country.Id, producerInfo, producerFaker));
        }

        for (var i = 10; i < 13; i++)
        {
            producers[i].Description = $"Updated description {i}";
            producers[i].IsEditForId = producers[1].Id;
        }

        modelBuilder.Entity<Producer>().HasData(producers);
        return producers;
    }

    private static List<Producer> GenerateProducersForCountry(
        string seedString,
        Guid countryId,
        List<(string Name, string City, string Description)> producerInfo,
        Faker<Producer> producerFaker)
    {
        Randomizer.Seed = SeedUtils.GetRandom(seedString);

        return producerInfo.Select(info =>
        {
            var producer = producerFaker.Generate();
            producer.Name = info.Name;
            producer.City = info.City;
            producer.Description = info.Description;
            producer.CountryId = countryId;
            return producer;
        }).ToList();
    }
}
