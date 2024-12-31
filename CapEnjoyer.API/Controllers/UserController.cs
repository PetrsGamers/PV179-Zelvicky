namespace CapEnjoyer.API.Controllers;

using BL.Services.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, ICouponService couponService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await userService.GetUserById(id);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        var createdUser = await userService.CreateUser(user);
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
    {
        var updatedUser = await userService.UpdateUser(id, user);
        return Ok(updatedUser);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await userService.DeleteUser(id);
        return Ok();
    }

    [HttpGet("{userId:guid}/coupons")]
    public async Task<IActionResult> GetUserCoupons(Guid userId)
    {
        var coupons = await couponService.GetCouponsByBuyerIdAsync(userId);
        return Ok(coupons);
    }
}
