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

/// <summary>
/// Тесты для проверки запросов к данным о рейсах
/// </summary>
public class FlightQueriesTests(FlightDataFixture fixture) : IClassFixture<FlightDataFixture>
{

    /// <summary>
    /// Проверка получения топ-5 рейсов по количеству пассажиров
    /// </summary>
    [Fact]
    public void Top5FlightsByPassengerCount_ShouldReturnCorrectResults()
    {
        var result = fixture.Tickets
            .GroupBy(t => t.FlightId)
            .Select(g => new
            {
                Flight = fixture.Flights.First(f => f.Id == g.Key),
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

    /// <summary>
    /// Проверка получения рейсов с минимальной продолжительностью
    /// </summary>
    [Fact]
    public void FlightsWithMinDuration_ShouldReturnCorrectResults()
    {
        var minDuration = fixture.Flights.Min(f => f.Duration);
        var result = fixture.Flights
            .Where(f => f.Duration == minDuration)
            .ToList();

        Assert.All(result, f => Assert.Equal(minDuration, f.Duration));
        Assert.True(result.Count > 0);
        Assert.All(fixture.Flights.Where(f => !result.Contains(f)), 
            f => Assert.True(f.Duration > minDuration));
    }

    /// <summary>
    /// Проверка получения пассажиров без багажа на выбранном рейсе
    /// </summary>
    [Fact]
    public void PassengersWithZeroLuggageOnSelectedFlight_ShouldReturnCorrectResults()
    {
        var selectedFlight = fixture.Flights[0];
        var result = fixture.Tickets
            .Where(t => t.FlightId == selectedFlight.Id && t.LuggageWeight == 0)
            .Select(t => t.Passenger)
            .OrderBy(p => p.FullName)
            .ToList();

        Assert.All(result, p => 
        {
            var ticket = fixture.Tickets.First(t => t.PassengerId == p.Id && t.FlightId == selectedFlight.Id);
            Assert.Equal(0, ticket.LuggageWeight);
        });
        
        for (int i = 0; i < result.Count - 1; i++)
        {
            Assert.True(string.Compare(result[i].FullName, result[i + 1].FullName) <= 0);
        }
    }

    /// <summary>
    /// Проверка фильтрации рейсов по модели самолета и периоду времени
    /// </summary>
    [Fact]
    public void FlightsByAircraftModelAndTimePeriod_ShouldReturnCorrectResults()
    {
        var selectedModel = fixture.Models[0];
        var startDate = new DateTime(2024, 1, 14);
        var endDate = new DateTime(2024, 1, 16);

        var result = fixture.Flights
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
            Assert.Equal(selectedModel.Id, fixture.Flights.First(fl => fl.Code == f.FlightCode).AircraftModelId);
            Assert.True(f.DepartureDate >= startDate);
            Assert.True(f.DepartureDate <= endDate);
        });
    }

    /// <summary>
    /// Проверка поиска рейсов по пункту вылета и назначения
    /// </summary>
    [Fact]
    public void FlightsByDepartureAndArrivalPoints_ShouldReturnCorrectResults()
    {
        var departurePoint = "Moscow";
        var arrivalPoint = "St. Petersburg";

        var result = fixture.Flights
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