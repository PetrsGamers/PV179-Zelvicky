namespace DAL.BL.Controllers;
using Microsoft.AspNetCore.Mvc;
using DTOs;
using Entities;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ApplicationContext context, ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        try
        {
            var users = context.Users.ToList();
            return this.Ok(users);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDetailDto userDto)
    {
        try
        {
            if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                logger.LogError("Username, Email, and Password are required.");
                return this.BadRequest("Username, Email, and Password are required.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password,
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDetailDto userDto)
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return this.StatusCode(500, "Internal server error.");
        }
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
            logger.LogError($"User with ID {id} not found.");
            return this.NotFound($"User with ID {id} not found.");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return this.NoContent();
    }
}
