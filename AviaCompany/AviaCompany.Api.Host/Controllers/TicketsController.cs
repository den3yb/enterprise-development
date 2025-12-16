using Microsoft.AspNetCore.Mvc;
using AviaCompany.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace AviaCompany.Api.Host.Controllers;

/// <summary>
/// Контроллер для управления билетами: CRUD
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TicketsController(IApplicationService<TicketDto, TicketCreateUpdateDto, int> service, ILogger<TicketsController> logger) : ControllerBase
{
    /// <summary>
    /// Получить все билеты
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TicketDto>), 200)]
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
    /// Получить билет по ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TicketDto), 200)]
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
    /// Создать билет
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), 201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Create([FromBody] TicketCreateUpdateDto dto)
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
    /// Обновить билет
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TicketDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> Update(int id, [FromBody] TicketCreateUpdateDto dto)
    {
        logger.LogInformation("{method} method of {controller} is called with {id}", nameof(Update), GetType().Name, id);
        try
        {
            var result = await service.Update(dto, id);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Update), GetType().Name);
            return Ok(result);
        }
         catch (ArgumentException ex)
        {
            logger.LogWarning("Invalid argument during {method} in {controller}: {@exception}", nameof(Update), GetType().Name, ex);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Update), GetType().Name, ex);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Удалить билет
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