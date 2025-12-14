// AviaCompany.Application/Services/PassengerService.cs

using AutoMapper;
using AviaCompany.Application.Contracts;
using AviaCompany.Domain;

namespace AviaCompany.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций над сущностью Passenger
/// </summary>
public class PassengerService(
    IRepository<Passenger, int> repository,
    IMapper mapper
) : IApplicationService<PassengerDto, PassengerCreateUpdateDto, int>
{
    /// <summary>
    /// Создает новую сущность Passenger
    /// </summary>
    public async Task<PassengerDto> Create(PassengerCreateUpdateDto dto)
    {
        var entity = mapper.Map<Passenger>(dto);
        repository.Create(entity);
        return mapper.Map<PassengerDto>(entity);
    }

    /// <summary>
    /// Получает сущность Passenger по ID
    /// </summary>
    public async Task<PassengerDto?> Get(int dtoId)
    {
        var entity = repository.Get(dtoId);
        return entity != null ? mapper.Map<PassengerDto>(entity) : null;
    }

    /// <summary>
    /// Получает все сущности Passenger
    /// </summary>
    public async Task<IList<PassengerDto>> GetAll()
    {
        var entities = repository.GetAll();
        return entities.Select(mapper.Map<PassengerDto>).ToList();
    }

    /// <summary>
    /// Обновляет сущность Passenger по ID
    /// </summary>
    public async Task<PassengerDto> Update(PassengerCreateUpdateDto dto, int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) throw new ArgumentException($"Passenger с ID {dtoId} не найден");

        mapper.Map(dto, entity);
        repository.Update(entity);
        return mapper.Map<PassengerDto>(entity);
    }

    /// <summary>
    /// Удаляет сущность Passenger по ID
    /// </summary>
    public async Task<bool> Delete(int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) return false;

        repository.Delete(dtoId);
        return true;
    }
}