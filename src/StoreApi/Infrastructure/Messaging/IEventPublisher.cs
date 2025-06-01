namespace StoreApi.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<T>(string topic, T data, CancellationToken cancellationToken = default);
}