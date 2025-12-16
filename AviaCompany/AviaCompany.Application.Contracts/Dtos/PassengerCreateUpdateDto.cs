using System;

namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания пасажиров
/// </summary>
public record PassengerCreateUpdateDto(
    /// <summary>
    /// Номер паспорта
    /// </summary>    
    string PassportNumber,

    /// <summary>
    /// Полное имя
    /// </summary>    
    string FullName,

    /// <summary>
    /// Дата рождения
    /// </summary>    
    DateOnly BirthDate
);