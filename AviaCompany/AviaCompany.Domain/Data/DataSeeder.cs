namespace AviaCompany.Domain;

public class DataSeeder
{
    public List<AircraftFamily> Families { get; private set; }
    public List<AircraftModel> Models { get; private set; }
    public List<Flight> Flights { get; private set; }
    public List<Passenger> Passengers { get; private set; }
    public List<Ticket> Tickets { get; private set; }

    public DataSeeder()
    {
        Families = DataGenerator.GenerateAircraftFamilies();
        Models = DataGenerator.GenerateAircraftModels(Families);
        Flights = DataGenerator.GenerateFlights(Models);
        Passengers = DataGenerator.GeneratePassengers();
        Tickets = DataGenerator.GenerateTicket(Flights, Passengers);
    }
}