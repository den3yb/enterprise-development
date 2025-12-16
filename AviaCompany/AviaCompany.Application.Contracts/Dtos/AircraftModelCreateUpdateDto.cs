namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания модели самолетов
/// </summary>
public record AircraftModelCreateUpdateDto(
    string Name,
    double FlightRange,
    int PassengerCapacity,
    double CargoCapacity,
    int AircraftFamilyId
);