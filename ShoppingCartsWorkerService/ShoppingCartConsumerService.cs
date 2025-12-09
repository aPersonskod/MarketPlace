using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Interfaces;
using ShoppingCartsWorkerService.Settings;

namespace ShoppingCartsWorkerService;

public class ShoppingCartConsumerService(IOptions<ShoppingCartKafkaSettings> options, IBuyService buyService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig()
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = options.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(options.Value.Topic);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(TimeSpan.FromSeconds(3));
                if (consumeResult == null) continue;
                var cart = JsonSerializer.Deserialize<CartDto>(consumeResult.Message.Value);
                if (cart != null) await buyService.BuyCart(cart);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}