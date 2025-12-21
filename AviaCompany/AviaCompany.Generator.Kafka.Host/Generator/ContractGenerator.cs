using AviaCompany.Application.Contracts;
using Bogus;

namespace AviaCompany.Generator.Kafka.Host.Generator;

/// <summary>
/// Генератор тестовых данных для рейсов
/// </summary>
public static class ContractGenerator
{
    /// <summary>
    /// Генерирует пакет рейсов со случайными параметрами
    /// </summary>
    public static List<FlightCreateUpdateDto> GenerateFlights(
        int count, 
        IList<int> modelIds,
        IList<string> departurePoints,
        IList<string> arrivalPoints)
    {
        return new Faker<FlightCreateUpdateDto>()
            .CustomInstantiator(f => new FlightCreateUpdateDto(
                Code: $"FL{f.Random.Number(1000, 9999)}",
                DeparturePoint: f.PickRandom(departurePoints),
                ArrivalPoint: f.PickRandom(arrivalPoints),
                DepartureDate: f.Date.Future(30).ToUniversalTime(),
                ArrivalDate: f.Date.Future(31).ToUniversalTime(),
                Duration: TimeSpan.FromHours(f.Random.Double(1, 12)),
                AircraftModelId: f.PickRandom(modelIds)
            ))
            .Generate(count);
    }
}