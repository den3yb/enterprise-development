using Confluent.Kafka;
using System.Text.Json;

namespace AviaCompany.Infrastructure.Kafka.Deserializers;

/// <summary>
/// Десериализатор ключа сообщения Kafka для генератора рейсов
/// </summary>
public class FlightKeyDeserializer : IDeserializer<Guid>
{
    /// <summary>
    /// Преобразует полученные от Kafka байты ключа сообщения из формата JSON в значение типа Guid
    /// </summary>
    public Guid Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) =>
        JsonSerializer.Deserialize<Guid>(data);
}