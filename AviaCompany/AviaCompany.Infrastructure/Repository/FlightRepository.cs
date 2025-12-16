using Microsoft.EntityFrameworkCore;
using AviaCompany.Domain;

namespace AviaCompany.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями полетов,
/// реализующий базовые операции CRUD
/// </summary>
public class FlightRepository(AppDbContext dbContext) : IRepository<Flight, int>
{
    /// <summary>
    /// Функция создает новое семество и сохраняет в базе
    /// </summary>
    public void Create(Flight entity)
    {
        dbContext.Flights.Add(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция получает семество по айди
    /// </summary>
    public Flight? Get(int entityId)
    {
        return dbContext.Flights
            .Include(f => f.AircraftModel)
                .ThenInclude(am => am.AircraftFamily)
            .FirstOrDefault(e => e.Id == entityId);
    }

    /// <summary>
    /// Функция получает все семейства
    /// </summary>
    public List<Flight> GetAll()
    {
        return dbContext.Flights
            .Include(f => f.AircraftModel)
                .ThenInclude(am => am.AircraftFamily)
            .ToList();
    }

    /// <summary>
    /// Функция обновляет запись семейства 
    /// </summary>
    public void Update(Flight entity)
    {
        dbContext.Flights.Update(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция удаляет семейство по айди
    /// </summary>
    public void Delete(int entityId)
    {
        var entity = dbContext.Flights.FirstOrDefault(e => e.Id == entityId);
        if (entity != null)
        {
            dbContext.Flights.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}