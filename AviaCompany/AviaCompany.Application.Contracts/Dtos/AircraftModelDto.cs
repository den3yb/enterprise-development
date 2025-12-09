namespace AviaCompany.Application.Contracts.DTOs.AircraftModel;


/// <summary>
/// DTO для отображени информации о модели самолетов
/// </summary>
public record AircraftModelDto(
    int Id,
    string Name,
    double FlightRange,
    int PassengerCapacity,
    double CargoCapacity,
    int AircraftFamilyId
);

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