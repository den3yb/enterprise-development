// AviaCompany.Application/Services/FlightService.cs

using AutoMapper;
using AviaCompany.Application.Contracts;
using AviaCompany.Application.Contracts.DTOs.Flight;
using AviaCompany.Domain;

namespace AviaCompany.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций над сущностью Flight
/// </summary>
public class FlightService(
    IRepository<Flight, int> repository,
    IMapper mapper
) : IApplicationService<FlightDto, FlightCreateUpdateDto, int>
{
    /// <summary>
    /// Создает новую сущность Flight
    /// </summary>
    public async Task<FlightDto> Create(FlightCreateUpdateDto dto)
    {
        var entity = mapper.Map<Flight>(dto);
        repository.Create(entity);
        return mapper.Map<FlightDto>(entity);
    }

    /// <summary>
    /// Получает сущность Flight по ID
    /// </summary>
    public async Task<FlightDto?> Get(int dtoId)
    {
        var entity = repository.Get(dtoId);
        return entity != null ? mapper.Map<FlightDto>(entity) : null;
    }

    /// <summary>
    /// Получает все сущности Flight
    /// </summary>
    public async Task<IList<FlightDto>> GetAll()
    {
        var entities = repository.GetAll();
        return entities.Select(mapper.Map<FlightDto>).ToList();
    }

    /// <summary>
    /// Обновляет сущность Flight по ID
    /// </summary>
    public async Task<FlightDto> Update(FlightCreateUpdateDto dto, int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) throw new ArgumentException($"Flight с ID {dtoId} не найден");

        mapper.Map(dto, entity);
        repository.Update(entity);
        return mapper.Map<FlightDto>(entity);
    }

    /// <summary>
    /// Удаляет сущность Flight по ID
    /// </summary>
    public async Task<bool> Delete(int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) return false;

        repository.Delete(dtoId);
        return true;
    }
}