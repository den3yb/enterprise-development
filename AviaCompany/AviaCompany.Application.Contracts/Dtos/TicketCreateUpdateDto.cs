namespace AviaCompany.Application.Contracts;

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