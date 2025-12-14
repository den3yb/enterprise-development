namespace AviaCompany.Domain;

/// <summary>
/// Класс для генерации и предоставления тестовых данных для инициализации базы данных.
/// </summary>
public class DataSeeder
{
    /// <summary>
    /// Список семейств воздушных судов для тестовых данных.
    /// </summary>
    public List<AircraftFamily> Families { get; private set; }
    
    /// <summary>
    /// Список моделей воздушных судов для тестовых данных.
    /// </summary>
    public List<AircraftModel> Models { get; private set; }
    
    /// <summary>
    /// Список рейсов для тестовых данных.
    /// </summary>
    public List<Flight> Flights { get; private set; }
    
    /// <summary>
    /// Список пассажиров для тестовых данных.
    /// </summary>
    public List<Passenger> Passengers { get; private set; }
    
    /// <summary>
    /// Список билетов для тестовых данных.
    /// </summary>
    public List<Ticket> Tickets { get; private set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DataSeeder"/> и генерирует тестовые данные.
    /// </summary>
    public DataSeeder()
    {
        Families = DataGenerator.GenerateAircraftFamilies();
        Models = DataGenerator.GenerateAircraftModels(Families);
        Flights = DataGenerator.GenerateFlights(Models);
        Passengers = DataGenerator.GeneratePassengers();
        Tickets = DataGenerator.GenerateTicket(Flights, Passengers);
    }
}