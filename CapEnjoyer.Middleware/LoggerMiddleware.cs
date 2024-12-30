namespace CapEnjoyer.Middleware;

using BL.Services.Interfaces;
using DAL.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
{
    private readonly RequestDelegate next = next;
    private readonly ILogger<LoggerMiddleware> logger = logger;
    private static readonly Action<ILogger, string, string, string, string?, Exception?> IncomingRequest =
        LoggerMessage.Define<string, string, string, string?>(
            LogLevel.Information,
            new EventId(1, nameof(IncomingRequest)),
            "LOGGER-{App}: Incoming request: {Method} {Url} {Username}");

    private static readonly Action<ILogger, int, Exception?> ResponseLog =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(2, nameof(ResponseLog)),
            "LOGGER: Response code: {Code}");

    public async Task InvokeAsync(HttpContext context, IMiddlewareLoggingService middlewareLoggingService)
    {
        var appSource = context.Request.Headers["X-App-Source"].FirstOrDefault() ?? "Unknown";

        IncomingRequest(logger, appSource, context.Request.Method, context.Request.Path.ToString(), context.User.Identity?.Name, null);

        await next(context);
        await middlewareLoggingService.LogMiddlewareAsync(MiddlewareLogAction.Response,
            $"{appSource} {context.Request.Method} {context.Request.Path} {context.User.Identity?.Name}");
        ResponseLog(logger, context.Response.StatusCode, null);
    }
}
