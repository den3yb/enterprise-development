using AviaCompany.Application.Contracts;
using AviaCompany.Generator.Kafka.Host.Generator;
using AviaCompany.Generator.Kafka.Host.Interfaces;
using AviaCompany.Generator.Kafka.Host.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AviaCompany.Generator.Kafka.Host.Controllers;

/// <summary>
/// Контроллер для генерации и отправки рейсов в Kafka
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GeneratorController(
    ILogger<GeneratorController> logger,
    IProducerService producerService,
    IOptions<FlightGeneratorOptions> options
) : ControllerBase
{
    private readonly FlightGeneratorOptions _options = options.Value;

    /// <summary>
    /// Генерирует рейсы и отправляет их в Kafka партиями
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<FlightCreateUpdateDto>>> Get(
        [FromQuery] int batchSize = 0,
        [FromQuery] int payloadLimit = 0,
        [FromQuery] int waitTime = 0)
    {
        // Если параметры переданы через query, используем их, иначе — значения по умолчанию из конфигурации
        var actualBatchSize = batchSize > 0 ? batchSize : _options.BatchSize;
        var actualPayloadLimit = payloadLimit > 0 ? payloadLimit : _options.PayloadLimit;
        var actualWaitTime = waitTime > 0 ? waitTime : _options.WaitTime;

        logger.LogInformation("Запрос генерации {limit} рейсов партиями по {batchSize} с задержкой {waitTime}с", 
            actualPayloadLimit, actualBatchSize, actualWaitTime);

        try
        {
            var result = new List<FlightCreateUpdateDto>(actualPayloadLimit);
            var counter = 0;

            // Проверяем, что данные для генерации не пустые
            if (_options.AircraftModelIds.Length == 0 || _options.DeparturePoints.Length == 0 || _options.ArrivalPoints.Length == 0)
            {
                logger.LogError("Ошибка конфигурации: отсутствуют AircraftModelIds, DeparturePoints или ArrivalPoints");
                return StatusCode(500, "Ошибка конфигурации: отсутствуют необходимые данные для генерации рейсов");
            }

            while (counter < actualPayloadLimit)
            {
                var currentBatchSize = Math.Min(actualBatchSize, actualPayloadLimit - counter);
                var batch = ContractGenerator.GenerateFlights(
                    currentBatchSize, 
                    _options.AircraftModelIds, 
                    _options.DeparturePoints, 
                    _options.ArrivalPoints
                );

                await producerService.SendAsync(batch);

                logger.LogInformation("Отправлена партия из {size} рейсов", currentBatchSize);
                counter += currentBatchSize;
                result.AddRange(batch);

                if (counter < actualPayloadLimit && actualWaitTime > 0)
                {
                    logger.LogInformation("Ожидание {waitTime}с перед следующей партией", actualWaitTime);
                    await Task.Delay(actualWaitTime * 1000);
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