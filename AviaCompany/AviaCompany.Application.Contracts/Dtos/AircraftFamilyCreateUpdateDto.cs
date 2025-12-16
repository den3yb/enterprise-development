namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания семейства самолетов
/// </summary>
public record AircraftFamilyCreateUpdateDto(
    /// <summary>
    /// Название семейства
    /// </summary>   
    string Name, 

    /// <summary>
    /// Производитель
    /// </summary>
    string Manufacturer
);