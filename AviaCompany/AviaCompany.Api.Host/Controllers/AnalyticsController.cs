using Microsoft.AspNetCore.Mvc;
using AviaCompany.Application.Contracts;

namespace AviaCompany.Api.Host.Controllers;

/// <summary>
/// Контроллер, предоставляющий доступ к аналитическим операциям:
/// топ-5 рейсов, минимальная продолжительность, пассажиры без багажа,
/// фильтрация по модели и периоду, вылет/назначение
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AnalyticsController(IAnalyticsService service, ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Возвращает топ-5 рейсов по количеству пассажиров
    /// </summary>
    [HttpGet("top-5-flights-by-passenger-count")]
    [ProducesResponseType(typeof(List<FlightDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetTop5FlightsByPassengerCount()
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(GetTop5FlightsByPassengerCount), GetType().Name);
        try
        {
            var result = await service.GetTop5FlightsByPassengerCountAsync();
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetTop5FlightsByPassengerCount), GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetTop5FlightsByPassengerCount), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Возвращает рейсы с минимальной продолжительностью
    /// </summary>
    [HttpGet("flights-with-minimal-duration")]
    [ProducesResponseType(typeof(List<FlightDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetFlightsWithMinimalDuration()
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(GetFlightsWithMinimalDuration), GetType().Name);
        try
        {
            var result = await service.GetFlightsWithMinimalDurationAsync();
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetFlightsWithMinimalDuration), GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetFlightsWithMinimalDuration), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Возвращает пассажиров без багажа на выбранном рейсе
    /// </summary>
    [HttpGet("passengers-without-baggage-on-flight")]
    [ProducesResponseType(typeof(List<PassengerDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetPassengersWithoutBaggageOnFlight([FromQuery] int flightId)
    {
        logger.LogInformation("{method} method of {controller} is called with {flightId}", nameof(GetPassengersWithoutBaggageOnFlight), GetType().Name, flightId);
        try
        {
            var result = await service.GetPassengersWithoutBaggageOnFlightAsync(flightId);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetPassengersWithoutBaggageOnFlight), GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetPassengersWithoutBaggageOnFlight), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Возвращает рейсы по модели самолета и периоду
    /// </summary>
    [HttpGet("flights-by-model-and-period")]
    [ProducesResponseType(typeof(List<FlightDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetFlightsByModelAndPeriod([FromQuery] int modelId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        logger.LogInformation("{method} method of {controller} is called with {modelId}, {startDate}, {endDate}", nameof(GetFlightsByModelAndPeriod), GetType().Name, modelId, startDate, endDate);
        try
        {
            var result = await service.GetFlightsByModelAndPeriodAsync(modelId, startDate, endDate);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetFlightsByModelAndPeriod), GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetFlightsByModelAndPeriod), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Возвращает рейсы по пункту вылета и назначения
    /// </summary>
    [HttpGet("flights-by-departure-and-arrival")]
    [ProducesResponseType(typeof(List<FlightDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetFlightsByDepartureAndArrival([FromQuery] string departurePoint, [FromQuery] string arrivalPoint)
    {
        logger.LogInformation("{method} method of {controller} is called with {departurePoint}, {arrivalPoint}", nameof(GetFlightsByDepartureAndArrival), GetType().Name, departurePoint, arrivalPoint);
        try
        {
            var result = await service.GetFlightsByDepartureAndArrivalAsync(departurePoint, arrivalPoint);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetFlightsByDepartureAndArrival), GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetFlightsByDepartureAndArrival), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }
}