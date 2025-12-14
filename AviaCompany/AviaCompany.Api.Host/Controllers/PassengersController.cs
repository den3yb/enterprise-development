using Microsoft.AspNetCore.Mvc;
using AviaCompany.Application.Contracts;

namespace AviaCompany.Api.Host.Controllers;

/// <summary>
/// Контроллер для управления пассажирами: CRUD
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PassengersController(IApplicationService<PassengerDto, PassengerCreateUpdateDto, int> service, ILogger<PassengersController> logger) : ControllerBase
{
    /// <summary>
    /// Получить всех пассажиров
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PassengerDto>), 200)]
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
    /// Получить пассажира по ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PassengerDto), 200)]
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
    /// Создать пассажира
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PassengerDto), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Create([FromBody] PassengerCreateUpdateDto dto)
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
    /// Обновить пассажира
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PassengerDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(int id, [FromBody] PassengerCreateUpdateDto dto)
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
    /// Удалить пассажира
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

