using System;

namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания полетова
public record FlightCreateUpdateDto(
    string Code,
    string DeparturePoint,
    string ArrivalPoint,
    DateTime DepartureDate,
    DateTime ArrivalDate,
    TimeSpan Duration,
    int AircraftModelId
);