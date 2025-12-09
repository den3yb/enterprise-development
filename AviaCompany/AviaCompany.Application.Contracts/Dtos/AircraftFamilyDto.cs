namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для отображени информации о семействе самолетов
/// </summary>
public record AircraftFamilyDto(int Id, string Name, string Manufacturer);

/// <summary>
/// DTO для обновления и создания семейства самолетов
/// </summary>
public record AircraftFamilyCreateUpdateDto(string Name, string Manufacturer);