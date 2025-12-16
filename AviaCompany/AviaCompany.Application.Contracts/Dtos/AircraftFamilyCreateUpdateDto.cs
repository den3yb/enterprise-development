namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания семейства самолетов
/// </summary>
public record AircraftFamilyCreateUpdateDto(string Name, string Manufacturer);