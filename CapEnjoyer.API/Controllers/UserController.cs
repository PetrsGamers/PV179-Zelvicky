namespace CapEnjoyer.API.Controllers;

using BL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await userService.GetAllUsers();
            return this.Ok(users);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var user = await userService.GetUserById(id);


            return this.Ok(user);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        try
        {
            var createdUser = await userService.CreateUser(user);


            return this.CreatedAtAction(nameof(this.GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
    {
        try
        {
            var updatedUser = await userService.UpdateUser(id, user);


            return this.Ok(updatedUser);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var deleted = userService.DeleteUser(id);


            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }


}
