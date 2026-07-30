using HostedService.Application.Interfaces.Repositories;
using HostedService.Application.Interfaces.Services;

namespace HostedService.Application.Services;

public class CartService(ICartRepository cartRepository) : ICartService
{
    public async Task SaveCachedCartsToDbAsync() => await cartRepository.SaveCachedCartsToDbAsync();
}