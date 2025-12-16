namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для отображени информации о семействе самолетов
/// </summary>
public record AircraftFamilyDto(
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    int Id, 

    /// <summary>
    /// Название семейства
    /// </summary>   
    string Name, 

    /// <summary>
    /// Производитель
    /// </summary>
    string Manufacturer
);
