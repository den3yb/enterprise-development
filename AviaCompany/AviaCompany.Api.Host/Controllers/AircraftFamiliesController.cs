// AviaCompany.Api.Host/Controllers/AircraftFamiliesController.cs

using Microsoft.AspNetCore.Mvc;
using AviaCompany.Application.Contracts;
using AviaCompany.Application.Contracts.DTOs.AircraftFamily;

namespace AviaCompany.Api.Host.Controllers;

/// <summary>
/// Контроллер для управления семействами самолётов: CRUD
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AircraftFamiliesController(IApplicationService<AircraftFamilyDto, AircraftFamilyCreateUpdateDto, int> service, ILogger<AircraftFamiliesController> logger) : ControllerBase
{
    /// <summary>
    /// Получить все семейства самолётов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<AircraftFamilyDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetAll()
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(GetAll), GetType().Name);
        try
        {
            var result = await service.GetAll();
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetAll), GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetAll), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Получить семейство самолёта по ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AircraftFamilyDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetById(int id)
    {
        logger.LogInformation("{method} method of {controller} is called with {id}", nameof(GetById), GetType().Name, id);
        try
        {
            var result = await service.Get(id);
            if (result == null) return NotFound();

            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetById), GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetById), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Создать семейство самолёта
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AircraftFamilyDto), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Create([FromBody] AircraftFamilyCreateUpdateDto dto)
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(Create), GetType().Name);
        try
        {
            var result = await service.Create(dto);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Create), GetType().Name);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Create), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Обновить семейство самолёта
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AircraftFamilyDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(int id, [FromBody] AircraftFamilyCreateUpdateDto dto)
    {
        logger.LogInformation("{method} method of {controller} is called with {id}", nameof(Update), GetType().Name, id);
        try
        {
            var result = await service.Update(dto, id);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Update), GetType().Name);
            return Ok(result);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Update), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Удалить семейство самолёта
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Delete(int id)
    {
        logger.LogInformation("{method} method of {controller} is called with {id}", nameof(Delete), GetType().Name, id);
        try
        {
            var result = await service.Delete(id);
            if (!result) return NotFound();

            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Delete), GetType().Name);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Delete), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }
}