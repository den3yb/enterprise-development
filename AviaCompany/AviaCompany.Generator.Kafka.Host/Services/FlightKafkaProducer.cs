using AviaCompany.Application.Contracts;
using AviaCompany.Generator.Kafka.Host.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;

namespace AviaCompany.Generator.Kafka.Host.Services;

public class FlightKafkaProducer(
    IProducer<Guid, IList<FlightCreateUpdateDto>> producer, 
    IConfiguration configuration,
    ILogger<FlightKafkaProducer> logger
) : IProducerService
{
    private readonly string _topicName = configuration["Kafka:TopicName"] ?? "flights-topic";
    private readonly IProducer<Guid, IList<FlightCreateUpdateDto>> _producer = producer;

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