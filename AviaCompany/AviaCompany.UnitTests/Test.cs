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
    public void GetTop5FlightsByPassengerCount_ReturnsFiveFlights()
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
        var isOrdered = result.SequenceEqual(result.OrderByDescending(x => x.PassengerCount));
        Assert.True(isOrdered, "Рейсы должны быть отсортированы по убыванию количества пассажиров");
        Assert.All(result, r => Assert.NotNull(r.Flight));
    }

    /// <summary>
    /// Проверка получения рейсов с минимальной продолжительностью
    /// </summary>
    [Fact]
    public void GetFlightsWithMinimalDuration_ReturnsExpectedFlights()
    {
        var minDuration = fixture.Flights.Min(f => f.Duration);
        var result = fixture.Flights
            .Where(f => f.Duration == minDuration)
            .ToList();

        Assert.All(result, f => Assert.Equal(minDuration, f.Duration));
        Assert.NotEmpty(result);
        Assert.All(fixture.Flights.Where(f => !result.Contains(f)), 
            f => Assert.True(f.Duration > minDuration));
    }

    /// <summary>
    /// Проверка получения пассажиров без багажа на выбранном рейсе
    /// </summary>
    [Fact]
    public void GetPassengersWithoutBaggage_ReturnsPassengers()
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
    }

    /// <summary>
    /// Проверка фильтрации рейсов по модели самолета и периоду времени
    /// </summary>
    [Fact]
    public void GetFlightsByModelAndPeriod_ReturnsCorrectFlights()
    {
        var selectedModel = fixture.Models[0];
        var startDate = new DateTime(2024, 1, 14);
        var endDate = new DateTime(2024, 1, 16);

        var result = fixture.Flights
            .Where(f => f.AircraftModelId == selectedModel.Id &&
                    f.DepartureDate >= startDate &&
                    f.DepartureDate <= endDate)
            .ToList();

        Assert.All(result, flight => 
        {
            Assert.Equal(selectedModel.Id, flight.AircraftModelId);
            Assert.True(flight.DepartureDate >= startDate);
            Assert.True(flight.DepartureDate <= endDate);
        });
    }

    /// <summary>
    /// Проверка поиска рейсов по пункту вылета и назначения
    /// </summary>
    [Fact]
    public void GetFlightsByDepartureArrivalPoint_ReturnsExpectedFlights()
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
        Assert.True(result.Count == 2); 
    }
}