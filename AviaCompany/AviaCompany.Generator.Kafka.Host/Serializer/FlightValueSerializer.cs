using AviaCompany.Application.Contracts;
using Confluent.Kafka;
using System.Text.Json;

namespace AviaCompany.Generator.Kafka.Host.Serializers;

/// <summary>
/// Сериализатор значения сообщения Kafka для пакетов рейсов
/// </summary>
public class FlightValueSerializer : ISerializer<IList<FlightCreateUpdateDto>>
{
    /// <summary>
    /// Преобразует список рейсов в JSON-байты для Kafka
    /// </summary>
    public byte[] Serialize(IList<FlightCreateUpdateDto> data, SerializationContext context) =>
        JsonSerializer.SerializeToUtf8Bytes(data);
}