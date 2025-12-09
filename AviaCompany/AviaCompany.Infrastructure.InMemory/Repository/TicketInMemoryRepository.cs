using AviaCompany.Domain;

namespace AirCompany.Infrastructure.InMemory.Repository;

public class TicketInMemoryRepository(List<Ticket> entities) : InMemoryRepository<Ticket, int>(entities)
{
    protected override int GetEntityId(Ticket entity) => entity.Id;

    protected override void SetEntityId(Ticket entity, int id) => entity.Id = id;
}