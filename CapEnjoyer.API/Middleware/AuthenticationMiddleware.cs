namespace CapEnjoyer.API.Middleware;

using BL.Services.Interfaces;
using DAL.Constants;

public class AuthenticationMiddleware(
    RequestDelegate next,
    ILogger<AuthenticationMiddleware> logger,
    IMiddlewareLoggingService middlewareLoggingService)
{
    private const string HardCodedToken = "token";

    private static readonly Action<ILogger, Exception?> LogUserAuthenticated =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1, nameof(LogUserAuthenticated)),
            "User authenticated successfully");

    private static readonly Action<ILogger, Exception?> LogUnauthorizedAccess =
        LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(2, nameof(LogUnauthorizedAccess)),
            "Unauthorized access attempt");

    public async Task InvokeAsync(HttpContext context)
    {
        var isAuthorizationHeaderPresent = context.Request.Headers.TryGetValue("Authorization", out var token);
        var isTokenValueCorrect = isAuthorizationHeaderPresent && token == HardCodedToken;

        if (isAuthorizationHeaderPresent && isTokenValueCorrect)
        {
            LogUserAuthenticated(logger, null);
            await next(context);
        }
        else
        {
            await middlewareLoggingService.LogMiddlewareAsync(MiddlewareLogAction.Error, "Unauthorized access attempt");
            LogUnauthorizedAccess(logger, null);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
