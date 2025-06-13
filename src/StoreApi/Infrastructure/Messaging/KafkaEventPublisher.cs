using System.Text.Json;
using Avro.Specific;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace StoreApi.Infrastructure.Messaging;

public sealed class KafkaEventPublisher : IEventPublisher
{
    private readonly IProducer<string, ISpecificRecord> _producer;
    private readonly ILogger<KafkaEventPublisher> _logger;

    public KafkaEventPublisher(IConfiguration config, ILogger<KafkaEventPublisher> logger)
    {
        _logger = logger;
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config["KAFKA_BOOTSTRAP_SERVERS"],
            Acks = Acks.All,
            EnableIdempotence = true,
        };

        var registryConfig = new SchemaRegistryConfig
        {
            Url = config["SCHEMA_REGISTRY_URL"],
        };
        
        var registry = new CachedSchemaRegistryClient(registryConfig);
        

        _producer = new ProducerBuilder<string, ISpecificRecord>(producerConfig)
            .SetValueSerializer(new AvroSerializer<ISpecificRecord>(registry))
            .Build();    }

    public async Task PublishAsync(string topic, ISpecificRecord message, CancellationToken cancellationToken = default)
    {
        await _producer.ProduceAsync(topic,
            new Message<string, ISpecificRecord>
            {
                Key   = Guid.NewGuid().ToString(),
                Value = message
            }, cancellationToken);

        _logger.LogInformation("→ {Topic}\n{Payload}", topic, message);
    }
}