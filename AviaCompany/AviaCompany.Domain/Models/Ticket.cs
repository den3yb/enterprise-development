namespace AviaCompany.Domain;

/// <summary>
/// Билет
/// </summary>
public class Ticket
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Номер места
    /// </summary>
    public required string SeatNumber { get; set; }

    /// <summary>
    /// Наличие ручной клади
    /// </summary>  
    public bool HasHandLuggage { get; set; }

    /// <summary>
    /// Вес багажа (в кг)
    /// </summary>
    public double LuggageWeight { get; set; }
    
    /// <summary>
    /// Идентификатор рейса
    /// </summary>
    public required int FlightId { get; set; }

    /// <summary>
    /// Рейс
    /// </summary>
    public required Flight Flight { get; set; }
    
    /// <summary>
    /// Идентификатор пассажира
    /// </summary>
    public required int PassengerId { get; set; }

    /// <summary>
    /// Пассажир
    /// </summary>
    public required Passenger Passenger { get; set; }
}