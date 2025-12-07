namespace Models.Interfaces;

public interface IKafkaProducer<TMessage> : IDisposable
{
    Task ProduceAsync(TMessage message, CancellationToken cancellationToken);
}