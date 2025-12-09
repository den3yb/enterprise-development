// AviaCompany.Infrastructure/Repositories/TicketRepository.cs

using Microsoft.EntityFrameworkCore;
using AviaCompany.Domain;

namespace AviaCompany.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями билетов,
/// реализующий базовые операции CRUD
/// </summary>
public class TicketRepository(AppDbContext dbContext) : IRepository<Ticket, int>
{
    /// <summary>
    /// Функция создает новое семество и сохраняет в базе
    /// </summary>
    public void Create(Ticket entity)
    {
        dbContext.Tickets.Add(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция получает семество по айди
    /// </summary>
    public Ticket? Get(int entityId)
    {
        return dbContext.Tickets
            .Include(t => t.Flight)
                .ThenInclude(f => f.AircraftModel)
                    .ThenInclude(am => am.AircraftFamily)
            .Include(t => t.Passenger)
            .FirstOrDefault(e => e.Id == entityId);
    }

    /// <summary>
    /// Функция получает все семейства
    /// </summary>
    public List<Ticket> GetAll()
    {
        return dbContext.Tickets
            .Include(t => t.Flight)
                .ThenInclude(f => f.AircraftModel)
                    .ThenInclude(am => am.AircraftFamily)
            .Include(t => t.Passenger)
            .ToList();
    }

    /// <summary>
    /// Функция обновляет запись семейства 
    /// </summary>
    public void Update(Ticket entity)
    {
        dbContext.Tickets.Update(entity);
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Функция удаляет семейство по айди
    /// </summary>
    public void Delete(int entityId)
    {
        var entity = dbContext.Tickets.FirstOrDefault(e => e.Id == entityId);
        if (entity != null)
        {
            dbContext.Tickets.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}