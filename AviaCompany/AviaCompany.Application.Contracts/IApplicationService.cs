// AviaCompany.Application.Contracts/IApplicationService.cs

namespace AviaCompany.Application.Contracts;

/// <summary>
/// Интерфейс службы приложения для CRUD операций
/// </summary>
public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Создание новой сущности на основе DTO
    /// </summary>
    public Task<TDto> Create(TCreateUpdateDto dto);

    /// <summary>
    /// Получение сущности по идентификатору
    /// </summary>
    public Task<TDto?> Get(TKey dtoId);

    /// <summary>
    /// Получение полного списка сущностей
    /// </summary>
    public Task<IList<TDto>> GetAll();

    /// <summary>
    /// Обновление сущности по идентификатору
    /// </summary>
    public Task<TDto> Update(TCreateUpdateDto dto, TKey dtoId);

    /// <summary>
    /// Удаление сущности по идентификатору
    /// </summary>
    public Task<bool> Delete(TKey dtoId);
}