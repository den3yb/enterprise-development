namespace AviaCompany.Domain;

/// <summary>
/// Пассажир
/// </summary>
public class Passenger
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Номер паспорта
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Полное имя
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public required DateOnly BirthDate { get; set; }
    
     /// <summary>
     /// Билеты пассажира
     /// </summary>
    public required List<Ticket> Ticket { get; set; } = [];
}