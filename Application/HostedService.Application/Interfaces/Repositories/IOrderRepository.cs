namespace HostedService.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task SaveCachedOrdersToDbAsync();
}