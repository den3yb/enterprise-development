using AutoMapper;
using AviaCompany.Application.Contracts;
using AviaCompany.Domain;

namespace AviaCompany.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций над сущностью AircraftFamily
/// </summary>
public class AircraftFamilyService(
    IRepository<AircraftFamily, int> repository,
    IMapper mapper
) : IApplicationService<AircraftFamilyDto, AircraftFamilyCreateUpdateDto, int>
{
    /// <summary>
    /// Создает новую сущность AircraftFamily
    /// </summary>
    public async Task<AircraftFamilyDto> Create(AircraftFamilyCreateUpdateDto dto)
    {
        var entity = mapper.Map<AircraftFamily>(dto);
        repository.Create(entity);
        return mapper.Map<AircraftFamilyDto>(entity);
    }

    /// <summary>
    /// Получает сущность AircraftFamily по ID
    /// </summary>
    public async Task<AircraftFamilyDto?> Get(int dtoId)
    {
        var entity = repository.Get(dtoId);
        return entity != null ? mapper.Map<AircraftFamilyDto>(entity) : null;
    }

    /// <summary>
    /// Получает все сущности AircraftFamily
    /// </summary>
    public async Task<IList<AircraftFamilyDto>> GetAll()
    {
        var entities = repository.GetAll();
        return entities.Select(mapper.Map<AircraftFamilyDto>).ToList();
    }

    /// <summary>
    /// Обновляет сущность AircraftFamily по ID
    /// </summary>
    public async Task<AircraftFamilyDto> Update(AircraftFamilyCreateUpdateDto dto, int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) throw new ArgumentException($"AircraftFamily с ID {dtoId} не найден");

        mapper.Map(dto, entity);
        repository.Update(entity);
        return mapper.Map<AircraftFamilyDto>(entity);
    }

    /// <summary>
    /// Удаляет сущность AircraftFamily по ID
    /// </summary>
    public async Task<bool> Delete(int dtoId)
    {
        var entity = repository.Get(dtoId);
        if (entity == null) return false;

        repository.Delete(dtoId);
        return true;
    }
}