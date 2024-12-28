namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Entities;
using DTOs;
using Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ColorService(CapEnjoyerDbContext context) : IColorService
{
    public async Task<IEnumerable<ColorDto>> GetColorsAsync()
    {
        var colors = await context.Colors
            .Select(c => c.Adapt<ColorDto>())
            .ToListAsync();
        return colors;
    }

    public async Task<ColorDto?> GetColorByIdAsync(Guid id)
    {
        var color = await context.Colors
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        return color.Adapt<ColorDto>();
    }

    public async Task<ColorDto> CreateColorAsync(ColorDto colorDto)
    {
        var color = colorDto.Adapt<Color>();
        await context.Colors.AddAsync(color);
        await context.SaveChangesAsync();
        return color.Adapt<ColorDto>();
    }

    public async Task<ColorDto> UpdateColorAsync(Guid id, ColorDto colorDto)
    {
        var color = await context.Colors.FindAsync(id) ?? throw new ArgumentException($"Color with ID {id} not found.");
        color.Name = colorDto.Name;
        color.HexCode = colorDto.HexCode;
        context.Colors.Update(color);
        await context.SaveChangesAsync();
        return color.Adapt<ColorDto>();
    }

    public async Task DeleteColorAsync(Guid id)
    {
        var color = await context.Colors.FindAsync(id) ?? throw new ArgumentException($"Color with ID {id} not found.");

        context.Colors.Remove(color);
        await context.SaveChangesAsync();
    }

    public async Task<List<SelectListItem>> GetColorOptionsAsync() =>
        await context.Colors
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Name} ({c.HexCode})" })
            .ToListAsync();
}
