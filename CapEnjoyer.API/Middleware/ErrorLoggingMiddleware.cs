namespace CapEnjoyer.API.Middleware;

using BL.Services.Interfaces;
using DAL.Constants;

public class ErrorLoggingMiddleware(
    RequestDelegate next,
    ILogger<ErrorLoggingMiddleware> logger,
    IMiddlewareLoggingService middlewareLoggingService)
{
    private static readonly Action<ILogger, string, Exception?> LogUnhandledException =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(LogUnhandledException)),
            "An unhandled exception occurred: {Message}");

    private static readonly Action<ILogger, Exception?> Log500StatusWithoutException =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(2, nameof(Log500StatusWithoutException)),
            "A 500 status code was encountered without an exception");

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            LogUnhandledException(logger, ex.Message, ex);
            await middlewareLoggingService.LogMiddlewareAsync(MiddlewareLogAction.Error, ex.Message);
            await HandleExceptionAsync(context);
        }

        if (context.Response is { StatusCode: StatusCodes.Status500InternalServerError, HasStarted: false })
        {
            Log500StatusWithoutException(logger, null);
            await middlewareLoggingService.LogMiddlewareAsync(MiddlewareLogAction.Error,
                "500 status code without exception");
            await HandleExceptionAsync(context);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var responseMessage = new
        {
            StatusCode = 500,
            Message =
                "The programmer made this code after more than 5 beers. Be patient, he will fix it sometimes."
        };

        return context.Response.WriteAsJsonAsync(responseMessage);
    }
}
