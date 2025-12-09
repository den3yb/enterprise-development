using Microsoft.EntityFrameworkCore;
using AviaCompany.Domain;

namespace AviaCompany.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями семейства самолетов,
/// реализующий базовые операции CRUD
/// </summary>
public class AircraftFamilyRepository(AppDbContext dbContext) : IRepository<AircraftFamily, int>
{
    /// <summary>
    /// Функция создает новое семество и сохраняет в базе
    /// </summary>
    public void Create(AircraftFamily entity)
    {
        dbContext.AircraftFamilies.Add(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция получает семество по айди
    /// </summary>
    public AircraftFamily? Get(int entityId)
    {
        return dbContext.AircraftFamilies.FirstOrDefault(e => e.Id == entityId);
    }

    /// <summary>
    /// Функция получает все семейства
    /// </summary>
    public List<AircraftFamily> GetAll()
    {
        return dbContext.AircraftFamilies.ToList();
    }

    /// <summary>
    /// Функция обновляет запись семейства 
    /// </summary>
    public void Update(AircraftFamily entity)
    {
        dbContext.AircraftFamilies.Update(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция удаляет семейство по айди
    /// </summary>
    public void Delete(int entityId)
    {
        var entity = dbContext.AircraftFamilies.FirstOrDefault(e => e.Id == entityId);
        if (entity != null)
        {
            dbContext.AircraftFamilies.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}