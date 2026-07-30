using HostedService.Application.Interfaces.Services;

namespace HostedService.Api;

public class Worker(
    ICartService cartService,
    IOrderService orderService,
    ILogger<Worker> logger) : BackgroundService
{
    private const int MinutesDelay = 8;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(MinutesDelay), stoppingToken);
            try
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await cartService.SaveCachedCartsToDbAsync();
                await orderService.SaveCachedOrdersToDbAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occured during attempting to save !!!");
            }
        }
    }
}