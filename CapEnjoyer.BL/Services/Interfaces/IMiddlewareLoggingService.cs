namespace CapEnjoyer.BL.Services.Interfaces;

using DAL.Constants;

public interface IMiddlewareLoggingService
{
    public Task LogMiddlewareAsync(MiddlewareLogAction action, string log);
}
