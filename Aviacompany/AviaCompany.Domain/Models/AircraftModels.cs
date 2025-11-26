using System.Collections.Generic;

namespace AviaCompany.Domain;

/// <summary>
/// Модель самолета
/// </summary>
public class AircraftModel
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Название модели
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Дальность полета (в км)
    /// </summary>
    public required double FlightRange { get; set; } 

    /// <summary>
    /// Вместимость пассажиров
    /// </summary>
    public required int PassengerCapacity { get; set; }
    
    /// <summary>
    /// Грузоподъемность (в тоннах)
    /// </summary>
    public required double CargoCapacity { get; set; } 

    /// <summary>
    /// Идентификатор семейства самолетов
    /// </summary>
    public required int AircraftFamilyId { get; set; }

    /// <summary>
    /// Семейство самолетов
    /// </summary>
    public required AircraftFamily AircraftFamily { get; set; }
    
    /// <summary>
    /// Рейсы выполняемые данной моделью
    /// </summary>
    public required List<Flight> Flights { get; set; } = [];
}