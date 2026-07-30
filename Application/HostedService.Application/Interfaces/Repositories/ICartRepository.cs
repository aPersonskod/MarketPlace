namespace HostedService.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task SaveCachedCartsToDbAsync();
}