using System;

namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания полетова
public record FlightCreateUpdateDto(
    /// <summary>
    /// Код рейса
    /// </summary> 
    string Code,

    /// <summary>
    /// Пункт вылета
    /// </summary>
    string DeparturePoint,

    /// <summary>
    /// Пункт назначения
    /// </summary>
    string ArrivalPoint,

    /// <summary>
    /// Дата вылета
    /// </summary>
    DateTime DepartureDate,

    /// <summary>
    /// Дата прибытия
    /// </summary>  
    DateTime ArrivalDate,

    /// <summary>
    /// Продолжительность полета
    /// </summary>
    TimeSpan Duration,

    /// <summary>
    /// Идентификатор модели самолета
    /// </summary>
    int AircraftModelId
);