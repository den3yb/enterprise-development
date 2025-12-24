using AviaCompany.Application.Contracts;
using AviaCompany.Generator.Kafka.Host.Interfaces;
using AviaCompany.Generator.Kafka.Host.Serializers;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AviaCompany.Generator.Kafka.Host.Services;

/// <summary>
/// Реализация продюсера Kafka для отправки рейсов в потоковом режиме с поддержкой ретраев при подключении
/// </summary>
public class FlightKafkaProducer(
    IConfiguration configuration,
    ILogger<FlightKafkaProducer> logger
) : IProducerService
{
    private readonly string _bootstrapServers = (configuration["KAFKA_BOOTSTRAP_SERVERS"] ?? "localhost:9092")
        .Replace("tcp://", "");
    private readonly string _topicName = configuration["Kafka:TopicName"] ?? "flights-topic";
    
    // Создаём продюсер при инициализации класса с ретраями
    private readonly IProducer<Guid, IList<FlightCreateUpdateDto>> _producer = CreateProducerWithRetries(
        configuration, 
        logger
    );

    private static readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<KafkaException>()
        .Or<Exception>()
        .WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                // Нужно создать логгер отдельно, потому что static метод не может использовать this.logger
            });

    /// <summary>
    /// Создаёт экземпляр Kafka продюсера с правильной конфигурацией и ретраями
    /// </summary>
    private static IProducer<Guid, IList<FlightCreateUpdateDto>> CreateProducerWithRetries(
        IConfiguration configuration, 
        ILogger<FlightKafkaProducer> logger)
    {
        var bootstrapServers = (configuration["KAFKA_BOOTSTRAP_SERVERS"] ?? "localhost:9092")
            .Replace("tcp://", "");
            
        var retryPolicy = Policy
            .Handle<KafkaException>()
            .Or<Exception>()
            .WaitAndRetry(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    logger.LogWarning("Попытка {retryCount} подключения к Kafka не удалась. Повтор через {timespan}", retryCount, timespan);
                });

        return retryPolicy.Execute(() =>
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ApiVersionRequest = false,
                MessageTimeoutMs = 10000,
                RequestTimeoutMs = 5000
            };

            return new ProducerBuilder<Guid, IList<FlightCreateUpdateDto>>(config)
                .SetKeySerializer(new FlightKeySerializer())
                .SetValueSerializer(new FlightValueSerializer())
                .Build();
        });
    }

    /// <summary>
    /// Асинхронно отправляет пакет рейсов в указанный Kafka топик с логированием и обработкой ошибок
    /// </summary>
    public async Task SendAsync(IList<FlightCreateUpdateDto> batch)
    {
        try
        {
            logger.LogInformation("Отправка пакета из {count} рейсов в топик {topic}", batch.Count, _topicName);
            
            var message = new Message<Guid, IList<FlightCreateUpdateDto>>
            {
                Key = Guid.NewGuid(),
                Value = batch
            };

            var result = await _producer.ProduceAsync(_topicName, message);
            logger.LogInformation("Сообщение доставлено в {partition} с offset {offset}", 
                result.TopicPartition, result.Offset);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при отправке пакета из {count} рейсов в топик {topic}", 
                batch.Count, _topicName);
            throw;
        }
    }
}