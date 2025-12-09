// AviaCompany.Application.Contracts/DTOs/Passenger/PassengerDto.cs

using System;

namespace AviaCompany.Application.Contracts.DTOs.Passenger;


/// <summary>
/// DTO для отображени информации о пасажирах
/// </summary>
public record PassengerDto(
    int Id,
    string PassportNumber,
    string FullName,
    DateOnly BirthDate
);

/// <summary>
/// DTO для обновления и создания пасажиров
/// </summary>
public record PassengerCreateUpdateDto(
    string PassportNumber,
    string FullName,
    DateOnly BirthDate
);