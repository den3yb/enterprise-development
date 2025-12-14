// AviaCompany.Application/Services/TicketService.cs

using AutoMapper;
using AviaCompany.Application.Contracts;
using AviaCompany.Domain;

namespace AviaCompany.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций над сущностью Ticket
/// </summary>
public class TicketService(
    IRepository<Ticket, int> repository,
    IMapper mapper
) : IApplicationService<TicketDto, TicketCreateUpdateDto, int>
{
    /// <summary>
    /// Создает новую сущность Ticket
    /// </summary>
    public async Task<TicketDto> Create(TicketCreateUpdateDto dto)
    {
        var entity = mapper.Map<Ticket>(dto);
        repository.Create(entity);
        return mapper.Map<TicketDto>(entity);
    }

    /// <summary>
    /// Получает сущность Ticket по ID
    /// </summary>
    public async Task<TicketDto?> Get(int dtoId)
    {
        var entity = repository.Get(dtoId);
        return entity != null ? mapper.Map<TicketDto>(entity) : null;
    }

    /// <summary>
    /// Получает все сущности Ticket
    /// </summary>
    public async Task<IList<TicketDto>> GetAll()
    {
        var entities = repository.GetAll();
        return entities.Select(mapper.Map<TicketDto>).ToList();
    }

    /// <summary>
    /// Обновляет сущность Ticket по ID
    /// </summary>
    public async Task<TicketDto> Update(TicketCreateUpdateDto dto, int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) throw new ArgumentException($"Ticket с ID {dtoId} не найден");

        mapper.Map(dto, entity);
        repository.Update(entity);
        return mapper.Map<TicketDto>(entity);
    }

    /// <summary>
    /// Удаляет сущность Ticket по ID
    /// </summary>
    public async Task<bool> Delete(int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) return false;

        repository.Delete(dtoId);
        return true;
    }
}