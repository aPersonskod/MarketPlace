using HostedService.Application.Interfaces.Repositories;
using HostedService.Application.Interfaces.Services;

namespace HostedService.Application.Services;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    public async Task SaveCachedOrdersToDbAsync() => await orderRepository.SaveCachedOrdersToDbAsync();
}