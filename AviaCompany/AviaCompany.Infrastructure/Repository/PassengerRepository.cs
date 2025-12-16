using Microsoft.EntityFrameworkCore;
using AviaCompany.Domain;

namespace AviaCompany.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями пасажиров,
/// реализующий базовые операции CRUD
/// </summary>
public class PassengerRepository(AppDbContext dbContext) : IRepository<Passenger, int>
{
    /// <summary>
    /// Функция создает новое семество и сохраняет в базе
    /// </summary>
    public void Create(Passenger entity)
    {
        dbContext.Passengers.Add(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция получает семество по айди
    /// </summary>
    public Passenger? Get(int entityId)
    {
        return dbContext.Passengers
            .FirstOrDefault(e => e.Id == entityId);
    }

    /// <summary>
    /// Функция получает все семейства
    /// </summary>
    public List<Passenger> GetAll()
    {
        return dbContext.Passengers.ToList();
    }

    /// <summary>
    /// Функция обновляет запись семейства 
    /// </summary>
    public void Update(Passenger entity)
    {
        dbContext.Passengers.Update(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция удаляет семейство по айди
    /// </summary>
    public void Delete(int entityId)
    {
        var entity = dbContext.Passengers.FirstOrDefault(e => e.Id == entityId);
        if (entity != null)
        {
            dbContext.Passengers.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}