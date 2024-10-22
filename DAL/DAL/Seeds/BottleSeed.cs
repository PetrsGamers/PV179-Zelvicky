namespace DAL.Seeds;

using System;
using System.Collections.Generic;
using Constants;
using Entities;
using Microsoft.EntityFrameworkCore;

public static class BottleSeed
{
    private static readonly Random Random = new();

    public static List<Bottle> Seed(ModelBuilder modelBuilder, List<Producer> producers)
    {
        var bottles = new List<Bottle>();
        var uniqueNames = new HashSet<string>();

        for (var i = 0; i < 150; i++)
        {
            bottles.Add(GenerateBottle(producers, uniqueNames));
        }

        foreach (var bottle in bottles)
        {
            modelBuilder.Entity<Bottle>().HasData(bottle);
        }

        return bottles;
    }

    private static Bottle GenerateBottle(List<Producer> producers, HashSet<string> uniqueNames)
    {
        string[] firstProperties = ["Small",
            "Large",
            "Beautiful",
            "Elegant",
            "Fancy",
            "Rustic",
            "Modern",
            "Antique",
            "Vibrant",
            "Classic",
            "Dark",
            "Light",
            "Sleek",
            "Bold",
            "Quaint",
            "Exotic",
            "Unique",
            "Delicate",
            "Sturdy",
            "Shiny",
            "Charming",
            "Glamorous",
            "Sapphire",
            "Emerald",
            "Crystal",
            "Amber",
            "Ceramic",
            "Glass",
            "Wooden"];

        string[] secondProperties = ["Baroque",
            "Pirate",
            "Vintage",
            "Royal",
            "Mystic",
            "Funky",
            "Elegant",
            "Bold",
            "Chic",
            "Rustic",
            "Traditional",
            "Artisan",
            "Cultural",
            "Futuristic",
            "Tropical",
            "Gothic",
            "Cosmic",
            "Zen",
            "Urban",
            "Majestic",
            "Serene",
            "Passionate",
            "Epic",
            "Legendary",
            "Whimsical",
            "Enchanting",
            "Daring",
            "Nautical",
            "Rugged",
            "Sophisticated"];

        string name;
        do
        {
            var first = firstProperties[Random.Next(firstProperties.Length)];
            var second = secondProperties[Random.Next(secondProperties.Length)];
            name = $"{first} {second} bottle";
        }
        while (uniqueNames.Contains(name));

        uniqueNames.Add(name);

        return new Bottle
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = $"A {name.ToLowerInvariant()} for various beverages.",
            Voltage = Math.Round(Random.NextDouble() * 12, 2),
            BottlePicture = "default_picture_url.jpg",
            DrinkType = GetRandomDrinkType(),
            ProducerId = producers[Random.Next(producers.Count)].Id,
        };
    }



    private static DrinkType GetRandomDrinkType()
    {
        var values = Enum.GetValues(typeof(DrinkType));
        return (DrinkType)(values.GetValue(Random.Next(values.Length)) ?? throw new InvalidOperationException());
    }
}
