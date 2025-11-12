using System.Collections.Generic;

namespace AviaCompany.Domain;

public class AircraftFamily
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Manufacturer { get; set; }
    
    public List<AircraftModel> Models { get; set; } = [];
}