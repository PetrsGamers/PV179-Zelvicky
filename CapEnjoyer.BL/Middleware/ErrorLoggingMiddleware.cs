namespace CapEnjoyer.BL.Middleware;

public class ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
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
            await HandleExceptionAsync(context);
        }

        if (context.Response.StatusCode == StatusCodes.Status500InternalServerError && !context.Response.HasStarted)
        {
            Log500StatusWithoutException(logger, null);
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
                "The programmer that made this code after more than 5 beers. Be patient, he will fix it sometimes."
        };

        return context.Response.WriteAsJsonAsync(responseMessage);
    }
}
