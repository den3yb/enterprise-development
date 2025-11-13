using System.Collections.Generic;

namespace AviaCompany.Domain;

public class AircraftModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double FlightRange { get; set; } 
    public required int PassengerCapacity { get; set; }
    public required double CargoCapacity { get; set; } 

    public required int AircraftFamilyId { get; set; }
    public required AircraftFamily AircraftFamily { get; set; }
    
    public List<Flight> Flights { get; set; } = [];
}