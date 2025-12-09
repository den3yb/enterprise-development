// AviaCompany.Infrastructure/Repositories/AircraftModelRepository.cs

using Microsoft.EntityFrameworkCore;
using AviaCompany.Domain;

namespace AviaCompany.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями модели самолетов,
/// реализующий базовые операции CRUD
/// </summary>
public class AircraftModelRepository(AppDbContext dbContext) : IRepository<AircraftModel, int>
{
    /// <summary>
    /// Функция создает новое модель и сохраняет в базе
    /// </summary>
    public void Create(AircraftModel entity)
    {
        dbContext.AircraftModels.Add(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция получает модель по айди
    /// </summary>
    public AircraftModel? Get(int entityId)
    {
        return dbContext.AircraftModels
            .Include(am => am.AircraftFamily)
            .FirstOrDefault(e => e.Id == entityId);
    }

    /// <summary>
    /// Функция получает все модели
    /// </summary>
    public List<AircraftModel> GetAll()
    {
        return dbContext.AircraftModels
            .Include(am => am.AircraftFamily)
            .ToList();
    }

    /// <summary>
    /// Функция обновляет запись модели 
    /// </summary>
    public void Update(AircraftModel entity)
    {
        dbContext.AircraftModels.Update(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция удаляет модель по айди
    /// </summary>
    public void Delete(int entityId)
    {
        var entity = dbContext.AircraftModels.FirstOrDefault(e => e.Id == entityId);
        if (entity != null)
        {
            dbContext.AircraftModels.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}