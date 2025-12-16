// AviaCompany.Application.Contracts/IAnalyticsService.cs

using AviaCompany.Application.Contracts;

namespace AviaCompany.Application.Contracts;

/// <summary>
/// Сервис для выполнения аналитических запросов по авиакомпании
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Получение топ-5 рейсов по количеству пассажиров
    /// </summary>
    public Task<IList<FlightDto>> GetTop5FlightsByPassengerCountAsync();

    /// <summary>
    /// Получение рейсов с минимальной продолжительностью
    /// </summary>
    public Task<IList<FlightDto>> GetFlightsWithMinimalDurationAsync();

    /// <summary>
    /// Получение пассажиров без багажа на выбранном рейсе
    /// </summary>
    public Task<IList<PassengerDto>> GetPassengersWithoutBaggageOnFlightAsync(int flightId);

    /// <summary>
    /// Получение рейсов по модели самолета и периоду времени
    /// </summary>
    public Task<IList<FlightDto>> GetFlightsByModelAndPeriodAsync(int modelId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получение рейсов по пункту вылета и назначения
    /// </summary>
    public Task<IList<FlightDto>> GetFlightsByDepartureAndArrivalAsync(string departurePoint, string arrivalPoint);
}