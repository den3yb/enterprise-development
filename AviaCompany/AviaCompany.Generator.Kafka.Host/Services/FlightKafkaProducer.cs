//https://localhost:7084/api/generator?batchSize=3&payloadLimit=6&waitTime=2
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
    private IProducer<Guid, IList<FlightCreateUpdateDto>>? _producer;

    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<KafkaException>()
        .Or<Exception>()
        .WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                logger.LogWarning("Попытка {retryCount} подключения к Kafka не удалась. Повтор через {timespan}", retryCount, timespan);
            });

    /// <summary>
    /// Получает или создаёт экземпляр Kafka Producer с ленивой инициализацией и автоматическими повторными попытками подключения
    /// </summary>
    private async Task<IProducer<Guid, IList<FlightCreateUpdateDto>>> GetProducerAsync()
    {
        if (_producer == null)
        {
            await _retryPolicy.ExecuteAsync(() =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = _bootstrapServers,
                    ApiVersionRequest = false,
                    MessageTimeoutMs = 10000,
                    RequestTimeoutMs = 5000
                };

                _producer = new ProducerBuilder<Guid, IList<FlightCreateUpdateDto>>(config)
                    .SetKeySerializer(new FlightKeySerializer())
                    .SetValueSerializer(new FlightValueSerializer())
                    .Build();

                return Task.CompletedTask;
            });
        }

        return _producer;
    }

    /// <summary>
    /// Асинхронно отправляет пакет рейсов в указанный Kafka топик с логированием и обработкой ошибок
    /// </summary>
    public async Task SendAsync(IList<FlightCreateUpdateDto> batch)
    {
        try
        {
            logger.LogInformation("Отправка пакета из {count} рейсов в топик {topic}", batch.Count, _topicName);
            
            var producer = await GetProducerAsync();
            var message = new Message<Guid, IList<FlightCreateUpdateDto>>
            {
                Key = Guid.NewGuid(),
                Value = batch
            };

            var result = await producer.ProduceAsync(_topicName, message);
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