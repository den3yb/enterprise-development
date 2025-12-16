namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для отображени информации о модели самолетов
/// </summary>
public record AircraftModelDto(
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    int Id,

    /// <summary>
    /// Название модели
    /// </summary>
    string Name,

    /// <summary>
    /// Дальность полета (в км)
    /// </summary>
    double FlightRange,

    /// <summary>
    /// Вместимость пассажиров
    /// </summary>   
    int PassengerCapacity,

    /// <summary>
    /// Грузоподъемность
    /// </summary>  
    double CargoCapacity,

    /// <summary>
    /// Идентификатор семейства самолетов
    /// </summary>
    int AircraftFamilyId
);