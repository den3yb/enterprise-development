using AviaCompany.Application.Contracts;
using Confluent.Kafka;
using System.Text.Json;

namespace AviaCompany.Infrastructure.Kafka.Deserializers;

/// <summary>
/// Пользовательский десериализатор Kafka для преобразования значений сообщений из формата JSON в коллекции контрактов рейсов
/// </summary>
public class FlightValueDeserializer : IDeserializer<IList<FlightCreateUpdateDto>>
{
    /// <summary>
    /// Десериализует значение сообщения Kafka из формата JSON в список контрактов рейсов
    /// </summary>
    public IList<FlightCreateUpdateDto> Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull) return [];
        return JsonSerializer.Deserialize<IList<FlightCreateUpdateDto>>(data) ?? [];
    }
}