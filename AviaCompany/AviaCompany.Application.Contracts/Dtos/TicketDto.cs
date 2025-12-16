namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для отображени информации о билетах
/// </summary>
public record TicketDto(
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    int Id,

    /// <summary>
    /// Номер места
    /// </summary>
    string SeatNumber,

    /// <summary>
    /// Наличие ручной клади
    /// </summary>  
    bool HasHandLuggage,

    /// <summary>
    /// Вес багажа
    /// </summary>
    double LuggageWeight,

    /// <summary>
    /// Идентификатор рейса
    /// </summary>
    int FlightId,

    /// <summary>
    /// Идентификатор пассажира
    /// </summary>
    int PassengerId
);