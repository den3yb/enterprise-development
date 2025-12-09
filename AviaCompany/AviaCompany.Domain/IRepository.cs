namespace AviaCompany.Domain;

/// <summary>
/// Интерфейс репозитория с базовыми CRUD операциями
/// </summary>
public interface IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    /// <summary>
    /// Создать новую сущность
    /// </summary>
    void Create(TEntity entity);

    /// <summary>
    /// Получить сущность по ID
    /// </summary>
    TEntity? Get(TKey entityId);

    /// <summary>
    /// Получить все сущности
    /// </summary>
    List<TEntity> GetAll();

    /// <summary>
    /// Обновить сущность
    /// </summary>
    void Update(TEntity entity);

    /// <summary>
    /// Удалить сущность по ID
    /// </summary>
    void Delete(TKey entityId);
}