// Options/FlightGeneratorOptions.cs

using System.ComponentModel.DataAnnotations;

namespace AviaCompany.Generator.Kafka.Host.Options;

/// <summary>
/// Настройки генератора рейсов
/// </summary>
public class FlightGeneratorOptions
{
    /// <summary>
    /// Максимальное количество рейсов в одной партии
    /// </summary>
    public int BatchSize { get; set; } = 10;

    /// <summary>
    /// Общее количество рейсов для генерации
    /// </summary>
    public int PayloadLimit { get; set; } = 100;

    /// <summary>
    /// Задержка между отправкой партий (в секундах)
    /// </summary>
    public int WaitTime { get; set; } = 1;

    /// <summary>
    /// Список ID моделей самолётов
    /// </summary>
    public int[] AircraftModelIds { get; set; } = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    /// <summary>
    /// Список точек отправления
    /// </summary>
    public string[] DeparturePoints { get; set; } = ["Moscow", "St. Petersburg", "Sochi", "New York", "Tokyo", "Berlin", "Paris"];

    /// <summary>
    /// Список точек прибытия
    /// </summary>
    public string[] ArrivalPoints { get; set; } = ["Helsinki", "London", "Istanbul", "Dubai", "Singapore", "Los Angeles", "Sydney"];
}