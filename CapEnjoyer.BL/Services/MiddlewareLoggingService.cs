namespace CapEnjoyer.BL.Services;

using DAL;
using DAL.Constants;
using DAL.Entities;
using Interfaces;

public class MiddlewareLoggingService(CapEnjoyerDbContext context) : IMiddlewareLoggingService
{
    public async Task LogMiddlewareAsync(MiddlewareLogAction action, string log)
    {
        var middlewareLog = new MiddlewareLog
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now.ToUniversalTime(),
            Action = action,
            Log = log
        };
        await context.MiddlewareLogs.AddAsync(middlewareLog);
        await context.SaveChangesAsync();
    }
}
