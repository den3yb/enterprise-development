using Confluent.Kafka;
using System.Text.Json;

namespace AviaCompany.Generator.Kafka.Host.Serializers;

/// <summary>
/// Сериализатор ключа сообщения Kafka для генератора рейсов
/// </summary>
public class FlightKeySerializer : ISerializer<Guid>
{
    /// <summary>
    /// Преобразует Guid в JSON-байты для Kafka
    /// </summary>
    public byte[] Serialize(Guid data, SerializationContext context) =>
        JsonSerializer.SerializeToUtf8Bytes(data);
}