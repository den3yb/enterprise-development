namespace AviaCompany.Application.Contracts;

/// <summary>
/// DTO для обновления и создания билеты
/// </summary>
public record TicketCreateUpdateDto(
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