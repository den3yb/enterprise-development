namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания модели самолетов
/// </summary>
public record AircraftModelCreateUpdateDto(
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