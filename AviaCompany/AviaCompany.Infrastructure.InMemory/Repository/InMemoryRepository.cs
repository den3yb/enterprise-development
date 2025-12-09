using AviaCompany.Domain;

namespace AirCompany.Infrastructure.InMemory.Repository;

/// <summary>
/// генерирует репозиторий в памяти, который реализует CRUD
/// </summary>
public abstract class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    private readonly List<TEntity> _entities;

    private int _currentId;

    protected InMemoryRepository(List<TEntity> entities)
    {
        _entities = entities ?? [];

        if (typeof(TKey) == typeof(int))
        {
            _currentId = _entities.Count != 0 ? _entities.Max(e => (int)(object)GetEntityId(e)) + 1 : 1;
        }
    }

    public virtual void Create(TEntity entity)
    {
        if (typeof(TKey) == typeof(int))
        {
            SetEntityId(entity, (TKey)(object)_currentId++);
        }
        _entities.Add(entity);
    }

    public virtual TEntity? Get(TKey entityId)
    {
        return _entities.FirstOrDefault(e => GetEntityId(e).Equals(entityId));
    }

    public virtual List<TEntity> GetAll() => [.. _entities];

    public virtual void Update(TEntity entity)
    {
        Delete(GetEntityId(entity));
        _entities.Add(entity);
    }

    public virtual void Delete(TKey entityId)
    {
        var entity = Get(entityId);
        if (entity != null)
            _entities.Remove(entity);
    }

    protected abstract TKey GetEntityId(TEntity entity);

    protected abstract void SetEntityId(TEntity entity, TKey id);
}