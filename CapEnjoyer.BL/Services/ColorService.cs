namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Entities;
using DTOs;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class ColorService(CapEnjoyerDbContext context) : IColorService
{
    public async Task<IEnumerable<ColorDto>> GetColorsAsync()
    {
        var colors = await context.Colors
            .Select(c => new ColorDto { Id = c.Id, Name = c.Name, HexValue = c.HexCode })
            .ToListAsync();
        return colors;
    }

    public async Task<ColorDto?> GetColorByIdAsync(Guid id)
    {
        var color = await context.Colors
            .Where(c => c.Id == id)
            .Select(c => new ColorDto { Id = c.Id, Name = c.Name, HexValue = c.HexCode })
            .FirstOrDefaultAsync();
        return color;
    }

    public async Task<ColorDto> CreateColorAsync(ColorDto colorDto)
    {
        var color = new Color { Id = Guid.NewGuid(), Name = colorDto.Name, HexCode = colorDto.HexValue };
        await context.Colors.AddAsync(color);
        await context.SaveChangesAsync();
        return colorDto;
    }

    public async Task<ColorDto> UpdateColorAsync(Guid id, ColorDto colorDto)
    {
        var color = await context.Colors.FindAsync(id) ?? throw new ArgumentException($"Color with ID {id} not found.");

        color.Name = colorDto.Name;
        color.HexCode = colorDto.HexValue;
        context.Colors.Update(color);
        await context.SaveChangesAsync();
        return colorDto;
    }

    public async Task DeleteColorAsync(Guid id)
    {
        var color = await context.Colors.FindAsync(id) ?? throw new ArgumentException($"Color with ID {id} not found.");

        context.Colors.Remove(color);
        await context.SaveChangesAsync();
    }
}
