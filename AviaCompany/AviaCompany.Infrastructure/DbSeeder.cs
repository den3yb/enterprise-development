using AviaCompany.Domain;
using Microsoft.EntityFrameworkCore;

namespace AviaCompany.Infrastructure;

/// <summary>
/// Сидер данных для PostgreSQL
/// Решает проблемы с автоинкрементными ID и последовательностями
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Сидит ВСЕ данные в правильном порядке
    /// </summary>
    public static async Task SeedAllAsync(AppDbContext context)
    {
        if (context.AircraftFamilies.Any()) return;

        var families = DataGenerator.GenerateAircraftFamilies();
        var models = DataGenerator.GenerateAircraftModels(families.ToList());
        var flights = DataGenerator.GenerateFlights(models.ToList());
        var passengers = DataGenerator.GeneratePassengers();
        var tickets = DataGenerator.GenerateTicket(flights.ToList(), passengers.ToList());

        await context.AircraftFamilies.AddRangeAsync(families);
        await context.AircraftModels.AddRangeAsync(models);
        await context.Flights.AddRangeAsync(flights);
        await context.Passengers.AddRangeAsync(passengers);
        await context.Tickets.AddRangeAsync(tickets);

        await context.SaveChangesAsync();

        await FixSequences(context);
    }

    /// <summary>
    /// Обновляет PostgreSQL sequence после вставки данных
    /// </summary>
    private static async Task FixSequences(AppDbContext context)
    {
        var tables = new[] { "aircraft_families", "aircraft_models", "flights", "passengers", "tickets" };

        foreach (var table in tables)
        {
            var sql = $@"
            SELECT setval(
                pg_get_serial_sequence('{table}', 'id'), 
                COALESCE((SELECT MAX(id) FROM {table}), 1), 
                true
            )";

            await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}