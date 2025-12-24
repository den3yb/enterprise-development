using AviaCompany.Application.Contracts;
using AviaCompany.Infrastructure.Kafka.Deserializers;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AviaCompany.Infrastructure.Kafka.Services;

/// <summary>
/// Фоновый потребитель Kafka, который подписывается на настроенный топик и обрабатывает пакеты сообщений />
/// </summary>
public sealed class FlightKafkaConsumer(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<FlightKafkaConsumer> logger
) : BackgroundService
{
    private readonly string _topicName = configuration["Kafka:TopicName"] ?? "flights-topic";

    /// <summary>
    /// Выполняет цикл потребления сообщений с автоматическим переподключением и обработкой недоступности топика
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServers = configuration.GetConnectionString("avia-kafka") ?? "localhost:9092";
        
        logger.LogInformation("Kafka bootstrap servers: {bootstrapServers}", bootstrapServers);
        logger.LogInformation("Kafka topic name: {topicName}", _topicName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var groupId = "flight-consumer-group-permanent";
                
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = bootstrapServers,
                    GroupId = groupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = true,
                    SocketTimeoutMs = 20000,
                    SessionTimeoutMs = 10000,
                    HeartbeatIntervalMs = 3000
                };

                using var consumer = new ConsumerBuilder<Guid, IList<FlightCreateUpdateDto>>(consumerConfig)
                    .SetKeyDeserializer(new FlightKeyDeserializer())
                    .SetValueDeserializer(new FlightValueDeserializer())
                    .Build();

                consumer.Subscribe(_topicName);
                logger.LogInformation("Consumer successfully subscribed to topic {topic} with GroupId {groupId}", _topicName, groupId);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        if (consumeResult?.Message?.Value is null || consumeResult.Message.Value.Count == 0)
                            continue;

                        logger.LogInformation(
                            "Consumed message {key} from topic {topic} (Partition: {partition}, Offset: {offset})",
                            consumeResult.Message.Key, 
                            _topicName,
                            consumeResult.TopicPartition.Partition,
                            consumeResult.Offset);

                        using var scope = scopeFactory.CreateScope();
                        var flightService = scope.ServiceProvider.GetRequiredService<IApplicationService<FlightDto, FlightCreateUpdateDto, int>>();

                        foreach (var contract in consumeResult.Message.Value)
                        {
                            try
                            {
                                await flightService.Create(contract);
                                logger.LogInformation("Successfully created flight {code} in database", contract.Code);
                            }
                            catch (Exception ex)
                            {
                                logger.LogWarning(ex, "Skipping invalid flight contract: Code={code}, AircraftModelId={aircraftModelId}", 
                                    contract.Code, contract.AircraftModelId);
                            }
                        }

                        consumer.Commit(consumeResult);
                        logger.LogInformation("Successfully processed and committed message {key} from topic {topic}", 
                            consumeResult.Message.Key, _topicName);
                    }
                    catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
                    {
                        logger.LogWarning("Topic {topic} is not available yet, waiting 5 seconds before retry...", _topicName);
                        await Task.Delay(5000, stoppingToken);
                        break; 
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        logger.LogInformation("Consumer operation cancelled due to shutdown request");
                        return;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to consume or process message from topic {topic}", _topicName);
                        await Task.Delay(2000, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Consumer encountered an unrecoverable error, restarting in 5 seconds...");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    /// <summary>
    /// Корректно останавливает потребитель при завершении работы приложения
    /// </summary>
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Kafka consumer is stopping gracefully...");
        await base.StopAsync(stoppingToken);
    }
}
