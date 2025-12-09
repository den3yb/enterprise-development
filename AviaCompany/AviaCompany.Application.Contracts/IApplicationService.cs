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
    Task<TDto> Create(TCreateUpdateDto dto);

    /// <summary>
    /// Получение сущности по идентификатору
    /// </summary>
    Task<TDto?> Get(TKey dtoId);

    /// <summary>
    /// Получение полного списка сущностей
    /// </summary>
    Task<IList<TDto>> GetAll();

    /// <summary>
    /// Обновление сущности по идентификатору
    /// </summary>
    Task<TDto> Update(TCreateUpdateDto dto, TKey dtoId);

    /// <summary>
    /// Удаление сущности по идентификатору
    /// </summary>
    Task<bool> Delete(TKey dtoId);
}