namespace DAL.Seeds;

using Entities;
using Microsoft.EntityFrameworkCore;

public static class ColorSeed
{
    public static List<Color> Seed(ModelBuilder modelBuilder)
    {
        // Otázka za zlatého bludišťáka: Odkud pochází tyto barvy?
        var colors = new List<Color>
        {
            new() { Id = Guid.NewGuid(), Name = "Black", HexCode = "#141519" },
            new() { Id = Guid.NewGuid(), Name = "Red", HexCode = "#a02722" },
            new() { Id = Guid.NewGuid(), Name = "Orange", HexCode = "#f07613" },
            new() { Id = Guid.NewGuid(), Name = "Green", HexCode = "#546d1b" },
            new() { Id = Guid.NewGuid(), Name = "Brown", HexCode = "#724728" },
            new() { Id = Guid.NewGuid(), Name = "Blue", HexCode = "#35399d" },
            new() { Id = Guid.NewGuid(), Name = "Purple", HexCode = "#792aac" },
            new() { Id = Guid.NewGuid(), Name = "Cyan", HexCode = "#158991" },
            new() { Id = Guid.NewGuid(), Name = "Light Gray", HexCode = "#8e8e86" },
            new() { Id = Guid.NewGuid(), Name = "Light Blue", HexCode = "#3aafd9" },
            new() { Id = Guid.NewGuid(), Name = "Gray", HexCode = "#3e4447" },
            new() { Id = Guid.NewGuid(), Name = "Pink", HexCode = "#ed8dac" },
            new() { Id = Guid.NewGuid(), Name = "Lime", HexCode = "#70b919" },
            new() { Id = Guid.NewGuid(), Name = "Yellow", HexCode = "#f8c527" },
            new() { Id = Guid.NewGuid(), Name = "White", HexCode = "#e9ecec" },
            new() { Id = Guid.NewGuid(), Name = "Magenta", HexCode = "#bd44b3" }
        };

        foreach (var color in colors)
        {
            modelBuilder.Entity<Color>().HasData(color);
        }

        return colors;
    }
}
