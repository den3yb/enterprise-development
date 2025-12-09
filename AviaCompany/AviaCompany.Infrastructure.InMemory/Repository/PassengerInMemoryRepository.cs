using AviaCompany.Domain;

namespace AirCompany.Infrastructure.InMemory.Repository;

public class PassengerInMemoryRepository(List<Passenger> entities) : InMemoryRepository<Passenger, int>(entities)
{
    protected override int GetEntityId(Passenger entity) => entity.Id;

    protected override void SetEntityId(Passenger entity, int id) => entity.Id = id;
}