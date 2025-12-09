using System;

namespace AviaCompany.Application.Contracts.DTOs.Flight;


/// <summary>
/// DTO для отображени информации о полетах
/// </summary>
public record FlightDto(
    int Id,
    string Code,
    string DeparturePoint,
    string ArrivalPoint,
    DateTime DepartureDate,
    DateTime ArrivalDate,
    TimeSpan Duration,
    int AircraftModelId
);

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