// AviaCompany.Application/Services/AircraftModelService.cs

using AutoMapper;
using AviaCompany.Application.Contracts;
using AviaCompany.Application.Contracts.DTOs.AircraftModel;
using AviaCompany.Domain;

namespace AviaCompany.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций над сущностью AircraftModel
/// </summary>
public class AircraftModelService(
    IRepository<AircraftModel, int> repository,
    IMapper mapper
) : IApplicationService<AircraftModelDto, AircraftModelCreateUpdateDto, int>
{
    /// <summary>
    /// Создает новую сущность AircraftModel
    /// </summary>
    public async Task<AircraftModelDto> Create(AircraftModelCreateUpdateDto dto)
    {
        var entity = mapper.Map<AircraftModel>(dto);
        repository.Create(entity);
        return mapper.Map<AircraftModelDto>(entity);
    }

    /// <summary>
    /// Получает сущность AircraftModel по ID
    /// </summary>
    public async Task<AircraftModelDto?> Get(int dtoId)
    {
        var entity = repository.Get(dtoId);
        return entity != null ? mapper.Map<AircraftModelDto>(entity) : null;
    }

    /// <summary>
    /// Получает все сущности AircraftModel
    /// </summary>
    public async Task<IList<AircraftModelDto>> GetAll()
    {
        var entities = repository.GetAll();
        return entities.Select(mapper.Map<AircraftModelDto>).ToList();
    }

    /// <summary>
    /// Обновляет сущность AircraftModel по ID
    /// </summary>
    public async Task<AircraftModelDto> Update(AircraftModelCreateUpdateDto dto, int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) throw new ArgumentException($"AircraftModel с ID {dtoId} не найден");

        mapper.Map(dto, entity);
        repository.Update(entity);
        return mapper.Map<AircraftModelDto>(entity);
    }

    /// <summary>
    /// Удаляет сущность AircraftModel по ID
    /// </summary>
    public async Task<bool> Delete(int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) return false;

        repository.Delete(dtoId);
        return true;
    }
}