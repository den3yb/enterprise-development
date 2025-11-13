namespace AviaCompany.Domain;

public class Ticket
{
    public int Id { get; set; }
    public required string SeatNumber { get; set; }
    public bool HasHandLuggage { get; set; }
    public double LuggageWeight { get; set; }
    
    public required int FlightId { get; set; }
    public required Flight Flight { get; set; }
    
    public required int PassengerId { get; set; }
    public required Passenger Passenger { get; set; }
}