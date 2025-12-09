using AviaCompany.Domain;

namespace AirCompany.Infrastructure.InMemory.Repository;

public class AircraftModelInMemoryRepository(List<AircraftModel> entities) : InMemoryRepository<AircraftModel, int>(entities)
{
    protected override int GetEntityId(AircraftModel entity) => entity.Id;

    protected override void SetEntityId(AircraftModel entity, int id) => entity.Id = id;
}