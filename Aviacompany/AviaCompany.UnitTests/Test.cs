using AviaCompany.Domain;
using Xunit;

namespace AviaCompany.UnitTests;

public class FlightQueriesTests
{
    private readonly List<AircraftFamily> _families;
    private readonly List<AircraftModel> _models;
    private readonly List<Flight> _flights;
    private readonly List<Passenger> _passengers;
    private readonly List<Ticket> _tickets;

    public FlightQueriesTests()
    {
        _families = TestDataGenerator.GenerateAircraftFamilies();
        _models = TestDataGenerator.GenerateAircraftModels(_families);
        _flights = TestDataGenerator.GenerateFlights(_models);
        _passengers = TestDataGenerator.GeneratePassengers();
        _tickets = TestDataGenerator.GenerateTicket(_flights, _passengers);
    }

    [Fact]
    public void Top5FlightsByPassengerCount_ShouldReturnCorrectResults()
    {
        var result = _tickets
            .GroupBy(t => t.FlightId)
            .Select(g => new
            {
                Flight = _flights.First(f => f.Id == g.Key),
                PassengerCount = g.Count()
            })
            .OrderByDescending(x => x.PassengerCount)
            .Take(5)
            .ToList();

        Assert.Equal(5, result.Count);

        Assert.True(result[0].PassengerCount >= result[1].PassengerCount);
        Assert.True(result[1].PassengerCount >= result[2].PassengerCount);
        Assert.True(result[2].PassengerCount >= result[3].PassengerCount);
        Assert.True(result[3].PassengerCount >= result[4].PassengerCount);
        
        Assert.All(result, r => Assert.NotNull(r.Flight));
    }

    [Fact]
    public void FlightsWithMinDuration_ShouldReturnCorrectResults()
    {
        var minDuration = _flights.Min(f => f.Duration);

        var result = _flights
            .Where(f => f.Duration == minDuration)
            .ToList();

        Assert.All(result, f => Assert.Equal(minDuration, f.Duration));
        Assert.True(result.Count > 0);
        Assert.All(_flights.Where(f => !result.Contains(f)), 
            f => Assert.True(f.Duration > minDuration));
    }

    [Fact]
    public void PassengersWithZeroLuggageOnSelectedFlight_ShouldReturnCorrectResults()
    {
        var selectedFlight = _flights[0];

        var result = _tickets
            .Where(t => t.FlightId == selectedFlight.Id && t.LuggageWeight == 0)
            .Select(t => t.Passenger)
            .OrderBy(p => p.FullName)
            .ToList();

        Assert.All(result, p => 
        {
            var ticket = _tickets.First(t => t.PassengerId == p.Id && t.FlightId == selectedFlight.Id);
            Assert.Equal(0, ticket.LuggageWeight);
        });
        
        for (int i = 0; i < result.Count - 1; i++)
        {
            Assert.True(string.Compare(result[i].FullName, result[i + 1].FullName) <= 0);
        }
    }

    [Fact]
    public void FlightsByAircraftModelAndTimePeriod_ShouldReturnCorrectResults()
    {
        var selectedModel = _models[0];
        var startDate = new DateTime(2024, 1, 14);
        var endDate = new DateTime(2024, 1, 16);

        var result = _flights
            .Where(f => f.AircraftModelId == selectedModel.Id &&
                       f.DepartureDate >= startDate &&
                       f.DepartureDate <= endDate)
            .Select(f => new
            {
                FlightCode = f.Code,
                DeparturePoint = f.DeparturePoint,
                DestinationPoint = f.ArrivalPoint,
                DepartureDate = f.DepartureDate,
                ArrivalDate = f.ArrivalDate,
                AircraftModel = f.AircraftModel.Name,
                Duration = f.Duration
            })
            .ToList();

        Assert.All(result, f => 
        {
            Assert.Equal(selectedModel.Id, _flights.First(fl => fl.Code == f.FlightCode).AircraftModelId);
            Assert.True(f.DepartureDate >= startDate);
            Assert.True(f.DepartureDate <= endDate);
        });
    }

    [Fact]
    public void FlightsByDepartureAndArrivalPoints_ShouldReturnCorrectResults()
    {
        var departurePoint = "Moscow";
        var arrivalPoint = "St. Petersburg";

        var result = _flights
            .Where(f => f.DeparturePoint == departurePoint && f.ArrivalPoint == arrivalPoint)
            .ToList();

        Assert.All(result, f => 
        {
            Assert.Equal(departurePoint, f.DeparturePoint);
            Assert.Equal(arrivalPoint, f.ArrivalPoint);
        });
        Assert.True(result.Count >= 2); 
    }
}