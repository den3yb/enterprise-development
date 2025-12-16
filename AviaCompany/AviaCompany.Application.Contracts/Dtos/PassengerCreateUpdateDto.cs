using System;

namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания пасажиров
/// </summary>
public record PassengerCreateUpdateDto(
    string PassportNumber,
    string FullName,
    DateOnly BirthDate
);