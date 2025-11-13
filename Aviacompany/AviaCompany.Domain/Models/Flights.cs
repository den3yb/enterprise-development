using System;
using System.Collections.Generic;

namespace AviaCompany.Domain;

public class Flight
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string DeparturePoint { get; set; }
    public required string ArrivalPoint { get; set; }
    public required DateTime DepartureDate { get; set; }
    public required DateTime ArrivalDate { get; set; }
    public required TimeSpan DepartureTime { get; set; }
    public required TimeSpan Duration { get; set; }
    
    public required int AircraftModelId { get; set; }
    public required AircraftModel AircraftModel { get; set; }
    
    public List<Ticket> Ticket { get; set; } = [];
}