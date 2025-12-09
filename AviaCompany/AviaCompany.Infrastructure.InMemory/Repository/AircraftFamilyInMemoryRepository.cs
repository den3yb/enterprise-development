using AviaCompany.Domain;

namespace AirCompany.Infrastructure.InMemory.Repository;

public class AircraftFamilyInMemoryRepository(List<AircraftFamily> entities): InMemoryRepository<AircraftFamily, int>(entities)
{
    protected override int GetEntityId(AircraftFamily entity) => entity.Id;
    
    protected override void SetEntityId(AircraftFamily entity, int id) => entity.Id = id;
}