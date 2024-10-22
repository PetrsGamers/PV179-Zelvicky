using Microsoft.AspNetCore.Mvc;

namespace DAL.BL.Controllers;

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
}
