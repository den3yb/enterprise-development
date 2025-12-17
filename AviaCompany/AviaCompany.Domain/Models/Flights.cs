using System;
using System.Collections.Generic;

namespace AviaCompany.Domain;

/// <summary>
/// Рейс
/// </summary>
public class Flight
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Код рейса
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Пункт вылета
    /// </summary>
    public required string DeparturePoint { get; set; }

    /// <summary>
    /// Пункт назначения
    /// </summary>
    public required string ArrivalPoint { get; set; }

    /// <summary>
    /// Дата вылета
    /// </summary>
    public required DateTime DepartureDate { get; set; }

    /// <summary>
    /// Дата прибытия
    /// </summary>
    public required DateTime ArrivalDate { get; set; }

    /// <summary>
    /// Продолжительность полета
    /// </summary>
    public required TimeSpan Duration { get; set; }
    
    /// <summary>
    /// Идентификатор модели самолета
    /// </summary>
    public required int AircraftModelId { get; set; }

    /// <summary>
    /// Модель самолета
    /// </summary>
    public required AircraftModel AircraftModel { get; set; }
    
    /// <summary>
    /// Билеты на рейс
    /// </summary>
    public List<Ticket>? Ticket { get; set; } = [];
}