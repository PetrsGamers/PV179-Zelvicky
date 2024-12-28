namespace CapEnjoyer.API.Middleware;

using BL.Services.Interfaces;
using DAL.Constants;

public class LoggerMiddleware(
    RequestDelegate next,
    ILogger<LoggerMiddleware> logger,
    IMiddlewareLoggingService middlewareLoggingService)
{
    private static readonly Action<ILogger, string, string, Exception?> IncomingRequest =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(1, nameof(IncomingRequest)),
            "LOGGER: Incoming request: {Method} {Url}");

    private static readonly Action<ILogger, int, Exception?> ResponseLog =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(2, nameof(ResponseLog)),
            "LOGGER: Response code: {Code}");

    public async Task InvokeAsync(HttpContext context)
    {
        IncomingRequest(logger, context.Request.Method, context.Request.Path.ToString(), null);

        await next(context);
        await middlewareLoggingService.LogMiddlewareAsync(MiddlewareLogAction.Response,
            $"{context.Request.Method} {context.Request.Path}");
        ResponseLog(logger, context.Response.StatusCode, null);
    }
}
