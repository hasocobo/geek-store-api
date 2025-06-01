using System.Text.Json;
using Confluent.Kafka;

namespace StoreApi.Infrastructure.Messaging;

public sealed class KafkaEventPublisher : IEventPublisher
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaEventPublisher> _logger;

    public KafkaEventPublisher(IConfiguration config, ILogger<KafkaEventPublisher> logger)
    {
        _logger = logger;
        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = config["KAFKA_BOOTSTRAP_SERVERS"],
            Acks = Acks.All,
            EnableIdempotence = true,
        };
        _producer = new ProducerBuilder<string, string>(kafkaConfig).Build();
    }

    public async Task PublishAsync<T>(string topic, T data, CancellationToken cancellationToken = default)
    {
        var serializedData = JsonSerializer.Serialize(data);
        _logger.LogInformation($"Publishing message to {topic}:  {serializedData}");
        await _producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = serializedData
        });
    }
}