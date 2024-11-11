namespace CapEnjoyer.BL.Middleware;

public class ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context);
        }

        if (context.Response.StatusCode == StatusCodes.Status500InternalServerError && !context.Response.HasStarted)
        {
            logger.LogError("A 500 status code was encountered without an exception.");
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
