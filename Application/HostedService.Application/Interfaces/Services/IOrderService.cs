namespace HostedService.Application.Interfaces.Services;

public interface IOrderService
{
    Task SaveCachedOrdersToDbAsync();
}