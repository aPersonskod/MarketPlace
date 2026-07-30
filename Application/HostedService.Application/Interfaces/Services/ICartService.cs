namespace HostedService.Application.Interfaces.Services;

public interface ICartService
{
    Task SaveCachedCartsToDbAsync();
}