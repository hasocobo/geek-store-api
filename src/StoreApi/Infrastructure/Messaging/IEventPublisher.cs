using Avro.Specific;

namespace StoreApi.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync(string topic, ISpecificRecord avroSpecificRecord, CancellationToken cancellationToken = default);
}