using AviaCompany.Domain;
using Xunit;

namespace AviaCompany.UnitTests;

/// <summary>
/// Fixture с данными для теста
/// </summary>
public class FlightDataFixture
{
    public List<AircraftFamily> Families { get; }
    public List<AircraftModel> Models { get; }
    public List<Flight> Flights { get; }
    public List<Passenger> Passengers { get; }
    public List<Ticket> Tickets { get; }

    public FlightDataFixture()
    {
        Families = DataGenerator.GenerateAircraftFamilies();
        Models = DataGenerator.GenerateAircraftModels(Families);
        Flights = DataGenerator.GenerateFlights(Models);
        Passengers = DataGenerator.GeneratePassengers();
        Tickets = DataGenerator.GenerateTicket(Flights, Passengers);
    }
}