namespace CapEnjoyer.BL.Controllers;

using DAL;
using DAL.Entities;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UsersController(CapEnjoyerDbContext context, ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return this.Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDetailDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Email) ||
            string.IsNullOrEmpty(userDto.Password))
        {
            return this.BadRequest("Username, Email, and Password are required.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(), Username = userDto.Username, Email = userDto.Email, Password = userDto.Password
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return this.CreatedAtAction(nameof(this.GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDetailDto userDto)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return this.NotFound($"User with ID {id} not found.");
        }


        user.Username = userDto.Username;

        user.Email = userDto.Email;

        user.Password = userDto.Password;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return this.NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return this.NotFound($"User with ID {id} not found.");
        }

        return this.Ok(user);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return this.NotFound($"User with ID {id} not found.");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return this.NoContent();
    }
}
