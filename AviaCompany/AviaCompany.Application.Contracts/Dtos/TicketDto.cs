namespace AviaCompany.Application.Contracts;


/// <summary>
/// DTO для отображени информации о билетах
/// </summary>
public record TicketDto(
    int Id,
    string SeatNumber,
    bool HasHandLuggage,
    double LuggageWeight,
    int FlightId,
    int PassengerId
);

/// <summary>
/// DTO для обновления и создания билеты
/// </summary>
public record TicketCreateUpdateDto(
    string SeatNumber,
    bool HasHandLuggage,
    double LuggageWeight,
    int FlightId,
    int PassengerId
);