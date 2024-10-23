namespace DAL.BL.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
{
    private const string HardCodedToken = "token";

    public async Task InvokeAsync(HttpContext context)
    {
        var isAuthorizationHeaderPresent = context.Request.Headers.TryGetValue("Authorization", out var token);
        var isTokenValueCorrect = isAuthorizationHeaderPresent && token == HardCodedToken;
        if (isAuthorizationHeaderPresent && isTokenValueCorrect)
        {
            logger.LogInformation("User authenticated successfully.");
            await next(context);
        }
        else
        {
            logger.LogWarning("Unauthorized access attempt.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
