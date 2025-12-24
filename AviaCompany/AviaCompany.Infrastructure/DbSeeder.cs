using AviaCompany.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AviaCompany.Infrastructure;

/// <summary>
/// Сидер данных для PostgreSQL
/// Решает проблемы с автоинкрементными ID и последовательностями
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Сидит семейства самолётов
    /// </summary>
    private static async Task SeedAircraftFamiliesAsync(AppDbContext context)
    {
        if (!context.AircraftFamilies.Any())
        {
            var families = DataGenerator.GenerateAircraftFamilies(); 
            await context.AircraftFamilies.AddRangeAsync(families);
            await context.SaveChangesAsync();
            await ResetSequenceAsync(context, "aircraft_families_id_seq", "aircraft_families");
        }
    }

    /// <summary>
    /// Сидит модели самолётов
    /// </summary>
    private static async Task SeedAircraftModelsAsync(AppDbContext context)
    {
        if (!context.AircraftModels.Any())
        {
            var families = await context.AircraftFamilies.ToListAsync();
            var models = DataGenerator.GenerateAircraftModels(families); 
            await context.AircraftModels.AddRangeAsync(models);
            await context.SaveChangesAsync();
            await ResetSequenceAsync(context, "aircraft_models_id_seq", "aircraft_models");
        }
    }

    /// <summary>
    /// Сидит перелёты
    /// </summary>
    private static async Task SeedFlightsAsync(AppDbContext context)
    {
        if (!context.Flights.Any())
        {
            var models = await context.AircraftModels.ToListAsync();
            var flights = DataGenerator.GenerateFlights(models); 
            await context.Flights.AddRangeAsync(flights);
            await context.SaveChangesAsync();
            await ResetSequenceAsync(context, "flights_id_seq", "flights");
        }
    }

    /// <summary>
    /// Сидит пасажиров
    /// </summary>
    private static async Task SeedPassengersAsync(AppDbContext context)
    {
        if (!context.Passengers.Any())
        {
            var passengers = DataGenerator.GeneratePassengers(); 
            await context.Passengers.AddRangeAsync(passengers);
            await context.SaveChangesAsync();
            await ResetSequenceAsync(context, "passengers_id_seq", "passengers");
        }
    }

    /// <summary>
    /// Сидит билетики
    /// </summary>
    private static async Task SeedTicketsAsync(AppDbContext context)
    {
        if (!context.Tickets.Any())
        {
            var flights = await context.Flights.ToListAsync();
            var passengers = await context.Passengers.ToListAsync();
            var tickets = DataGenerator.GenerateTicket(flights, passengers); 
            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();
            await ResetSequenceAsync(context, "tickets_id_seq", "tickets");
        }
    }

    /// <summary>
    /// Проверяет, является ли строка допустимым идентификатором PostgreSQL
    /// </summary>
    private static bool IsValidIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            return false;

        if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
            return false;

        for (int i = 0; i < identifier.Length; i++)
        {
            char c = identifier[i];
            if (!char.IsLetterOrDigit(c) && c != '_')
                return false;
        }

        return true;
    }

    /// <summary>
    /// Сбрасывает последовательность для автоинкремента ID после вставки сидов
    /// </summary>
    private static async Task ResetSequenceAsync(AppDbContext context, string sequenceName, string tableName)
    {
        // Санитизация имен таблиц и последовательностей
        if (!IsValidIdentifier(sequenceName) || !IsValidIdentifier(tableName))
        {
            throw new ArgumentException($"Недопустимое имя последовательности или таблицы: {sequenceName}, {tableName}");
        }

        try
        {
            await context.Database.ExecuteSqlRawAsync(
                $"SELECT setval('{sequenceName}', COALESCE((SELECT MAX(id) FROM {tableName}), 1));"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сбросе последовательности {sequenceName}: {ex.Message}");
            Console.WriteLine("Проверьте правильность имени последовательности в PostgreSQL");
        }
    }

    /// <summary>
    /// Сидит ВСЕ данные в правильном порядке
    /// </summary>
    public static async Task SeedAllAsync(AppDbContext context)
    {
        await SeedAircraftFamiliesAsync(context);
        await SeedAircraftModelsAsync(context);
        await SeedFlightsAsync(context);
        await SeedPassengersAsync(context);
        await SeedTicketsAsync(context);
    }
}