using AutoMapper;
using AviaCompany.Application.Contracts;
using AviaCompany.Domain;

namespace AviaCompany.Application.Services;

///<sumary>
/// Сервис предоставляет аналитические операции 
///</sumary>
public class AnalyticsService(
    IRepository<Ticket, int> ticketRepository,
    IRepository<Flight, int> flightRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : IAnalyticsService
{
    ///<sumary>
    /// Получает топ 5 полетов по количеству людей
    ///</sumary>
    public async Task<IList<FlightDto>> GetTop5FlightsByPassengerCountAsync()
    {
        var tickets = ticketRepository.GetAll();

        var grouped = tickets
            .GroupBy(t => t.FlightId)
            .Select(g => new { FlightId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var flightIds = grouped.Select(g => g.FlightId).ToList();
        var flights = flightRepository.GetAll().Where(f => flightIds.Contains(f.Id)).ToList();

        return flights.Select(mapper.Map<FlightDto>).ToList();
    }

    ///<sumary>
    /// Получает самый короткий перелёт
    ///</sumary>
    public async Task<IList<FlightDto>> GetFlightsWithMinimalDurationAsync()
    {
        var flights = flightRepository.GetAll();
        var minDuration = flights.Min(f => f.Duration);
        var result = flights.Where(f => f.Duration == minDuration).ToList();

        return result.Select(mapper.Map<FlightDto>).ToList();
    }

    ///<sumary>
    /// Получает всех пассажиров без багажа по айди полета
    ///</sumary>
    public async Task<IList<PassengerDto>> GetPassengersWithoutBaggageOnFlightAsync(int flightId)
    {
        var tickets = ticketRepository.GetAll()
            .Where(t => t.FlightId == flightId && t.LuggageWeight == 0)
            .ToList();

        var passengerIds = tickets.Select(t => t.PassengerId).ToList();
        var passengers = passengerRepository.GetAll()
            .Where(p => passengerIds.Contains(p.Id))
            .ToList();

        return passengers.Select(mapper.Map<PassengerDto>).ToList();
    }

    ///<sumary>
    ///  Получает полеты по заданной айди модели и по заднному периоду
    ///</sumary>
    public async Task<IList<FlightDto>> GetFlightsByModelAndPeriodAsync(int modelId, DateTime startDate, DateTime endDate)
    {
        var flights = flightRepository.GetAll()
            .Where(f => f.AircraftModelId == modelId &&
                        f.DepartureDate >= startDate &&
                        f.DepartureDate <= endDate)
            .ToList();

        return flights.Select(mapper.Map<FlightDto>).ToList();
    }

    ///<sumary>
    /// Получает перелеты по месту отправки и прилета
    ///</sumary>
    public async Task<IList<FlightDto>> GetFlightsByDepartureAndArrivalAsync(string departurePoint, string arrivalPoint)
    {
        var flights = flightRepository.GetAll()
            .Where(f => f.DeparturePoint == departurePoint && f.ArrivalPoint == arrivalPoint)
            .ToList();

        return flights.Select(mapper.Map<FlightDto>).ToList();
    }
}