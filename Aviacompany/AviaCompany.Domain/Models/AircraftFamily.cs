using System.Collections.Generic;

namespace AviaCompany.Domain;

/// <summary>Семейство самолетов</summary>
public class AircraftFamily
{
    /// <summary>Уникальный идентификатор</summary>
    public int Id { get; set; }

    /// <summary>Название семейства</summary>
    public required string Name { get; set; }

    /// <summary>Производитель</summary>
    public required string Manufacturer { get; set; }
    
    /// <summary>Модели принадлежащие этому семейству самолётов</summary>
    public List<AircraftModel> Models { get; set; } = [];
}