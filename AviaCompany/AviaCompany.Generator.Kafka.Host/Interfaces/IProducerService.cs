using AviaCompany.Application.Contracts;

namespace AviaCompany.Generator.Kafka.Host.Interfaces;

/// <summary>
/// Интерфейс сервиса отправки пакетов рейсов в Kafka
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// Асинхронная отправка пакета рейсов в Kafka
    /// </summary>
    Task SendAsync(IList<FlightCreateUpdateDto> batch);
}