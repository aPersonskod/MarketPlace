using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Models.Interfaces;
using ShoppingCart.Settings;

namespace ShoppingCart.Services;

public class ShoppingCartProducer<TMessage> : IKafkaProducer<TMessage>
{
    private readonly IProducer<string,TMessage> _producer;
    private readonly string _topic;

    public ShoppingCartProducer(IOptions<ShoppingCartKafkaSettings> options)
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = options.Value.BootstrapServers,
            AllowAutoCreateTopics = true,
            Acks = Acks.All
        };
        _producer = new ProducerBuilder<string, TMessage>(config)
            .SetValueSerializer(new KafkaJsonSerializer<TMessage>()).Build();
        _topic = options.Value.Topic;
    }
    
    public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken)
    {
        await _producer.ProduceAsync(_topic, new Message<string,TMessage>()
        {
            Key = Guid.NewGuid().ToString(),
            Value = message
        }, cancellationToken);
    }

    public void Dispose() => _producer.Dispose();
}

internal class KafkaJsonSerializer<TMessage> : ISerializer<TMessage>
{
    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}