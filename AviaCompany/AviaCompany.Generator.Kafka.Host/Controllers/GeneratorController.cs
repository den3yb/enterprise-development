using AviaCompany.Application.Contracts;
using AviaCompany.Generator.Kafka.Host.Generator;
using AviaCompany.Generator.Kafka.Host.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AviaCompany.Generator.Kafka.Host.Controllers;

/// <summary>
/// Контроллер для генерации и отправки рейсов в Kafka
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GeneratorController(
    ILogger<GeneratorController> logger,
    IProducerService producerService,
    IConfiguration configuration
) : ControllerBase
{
    /// <summary>
    /// Генерирует рейсы и отправляет их в Kafka партиями
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<FlightCreateUpdateDto>>> Get(
        [FromQuery] int batchSize = 10,
        [FromQuery] int payloadLimit = 100,
        [FromQuery] int waitTime = 1)
    {
        logger.LogInformation("Запрос генерации {limit} рейсов партиями по {batchSize} с задержкой {waitTime}с", 
            payloadLimit, batchSize, waitTime);

        try
        {
            var result = new List<FlightCreateUpdateDto>(payloadLimit);
            var counter = 0;

            // Получаем возможные значения из конфигурации
            var modelIds = configuration.GetSection("FlightGenerator:AircraftModelIds")
                .Get<int[]>() ?? Array.Empty<int>();
            
            var departurePoints = configuration.GetSection("FlightGenerator:DeparturePoints")
                .Get<string[]>() ?? Array.Empty<string>();
            
            var arrivalPoints = configuration.GetSection("FlightGenerator:ArrivalPoints")
                .Get<string[]>() ?? Array.Empty<string>();

            if (modelIds.Length == 0 || departurePoints.Length == 0 || arrivalPoints.Length == 0)
            {
                logger.LogError("Ошибка конфигурации: отсутствуют AircraftModelIds, DeparturePoints или ArrivalPoints");
                return StatusCode(500, "Ошибка конфигурации: отсутствуют необходимые данные для генерации рейсов");
            }

            while (counter < payloadLimit)
            {
                var currentBatchSize = Math.Min(batchSize, payloadLimit - counter);
                var batch = ContractGenerator.GenerateFlights(
                    currentBatchSize, 
                    modelIds, 
                    departurePoints, 
                    arrivalPoints
                );

                await producerService.SendAsync(batch);

                logger.LogInformation("Отправлена партия из {size} рейсов", currentBatchSize);
                counter += currentBatchSize;
                result.AddRange(batch);

                if (counter < payloadLimit && waitTime > 0)
                {
                    logger.LogInformation("Ожидание {waitTime}с перед следующей партией", waitTime);
                    await Task.Delay(waitTime * 1000);
                }
            }

            logger.LogInformation("Успешно сгенерировано и отправлено {count} рейсов", result.Count);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при генерации рейсов");
            return StatusCode(500, $"Ошибка генерации: {ex.Message}\n{ex.InnerException?.Message}");
        }
    }
}