using AviaCompany.Domain;

namespace AirCompany.Infrastructure.InMemory.Repository;

public class FlightInMemoryRepository(List<Flight> entities) : InMemoryRepository<Flight, int>(entities)
{  
    protected override int GetEntityId(Flight entity) => entity.Id;

    protected override void SetEntityId(Flight entity, int id) => entity.Id = id;
}