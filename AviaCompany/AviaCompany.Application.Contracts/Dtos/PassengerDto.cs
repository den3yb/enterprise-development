using System;

namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для отображени информации о пасажирах
/// </summary>
public record PassengerDto(
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    int Id,

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