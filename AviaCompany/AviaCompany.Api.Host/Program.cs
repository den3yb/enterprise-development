/// <summary>
/// Точка входа веб-приложения авиакомпании с настройкой зависимостей, миграций и middleware.
/// </summary>
using System.Threading;
using AviaCompany.Application;
using AviaCompany.Application.Contracts;
using AviaCompany.Application.Services;
using AviaCompany.Domain;
using AviaCompany.Infrastructure;
using AviaCompany.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Интеграция с Aspire для управления зависимостями.
/// </summary>
builder.AddServiceDefaults();

/// <summary>
/// Настройка генерации Swagger-документации.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Регистрация контроллеров API.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Настройка подключения к PostgreSQL через Aspire.
/// </summary>
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));

/// <summary>
/// Регистрация AutoMapper с профилем маппинга.
/// </summary>
builder.Services.AddAutoMapper(typeof(AviaCompanyProfile));

/// <summary>
/// Регистрация DataSeeder как singleton-сервиса.
/// </summary>
builder.Services.AddSingleton<DataSeeder>();

/// <summary>
/// Регистрация репозиториев для всех сущностей.
/// </summary>
builder.Services.AddScoped<IRepository<AircraftFamily, int>, AircraftFamilyRepository>();
builder.Services.AddScoped<IRepository<AircraftModel, int>, AircraftModelRepository>();
builder.Services.AddScoped<IRepository<Flight, int>, FlightRepository>();
builder.Services.AddScoped<IRepository<Passenger, int>, PassengerRepository>();
builder.Services.AddScoped<IRepository<Ticket, int>, TicketRepository>();

/// <summary>
/// Регистрация CRUD-сервисов приложения.
/// </summary>
builder.Services.AddScoped<IApplicationService<FlightDto, FlightCreateUpdateDto, int>, FlightService>();
builder.Services.AddScoped<IApplicationService<AircraftFamilyDto, AircraftFamilyCreateUpdateDto, int>, AircraftFamilyService>();
builder.Services.AddScoped<IApplicationService<AircraftModelDto, AircraftModelCreateUpdateDto, int>, AircraftModelService>();
builder.Services.AddScoped<IApplicationService<PassengerDto, PassengerCreateUpdateDto, int>, PassengerService>();
builder.Services.AddScoped<IApplicationService<TicketDto, TicketCreateUpdateDto, int>, TicketService>();

/// <summary>
/// Регистрация сервиса аналитики.
/// </summary>
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

/// <summary>
/// Применение миграций базы данных с механизмом повторных попыток и инициализация тестовых данных.
/// </summary>
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

    var retryCount = 0;
    const int maxRetries = 10;
    const int delayMs = 2000;

    while (retryCount < maxRetries)
    {
        try
        {
            context.Database.Migrate(); 
            break;
        }
        catch (Exception ex) when (ex is Npgsql.NpgsqlException or InvalidOperationException)
        {
            retryCount++;
            if (retryCount >= maxRetries)
                throw new Exception("Не удалось подключиться к базе данных после нескольких попыток.", ex);

            Console.WriteLine($"Попытка {retryCount} не удалась. Повтор через {delayMs} мс...");
            Thread.Sleep(delayMs);
        }
    }

    if (!context.AircraftFamilies.Any())
    {
        context.AircraftFamilies.AddRange(seeder.Families);
        context.AircraftModels.AddRange(seeder.Models);
        context.Flights.AddRange(seeder.Flights);
        context.Passengers.AddRange(seeder.Passengers);
        context.Tickets.AddRange(seeder.Tickets);
        context.SaveChanges();
        await context.Database.ExecuteSqlRawAsync(
            "SELECT setval('aircraft_families_id_seq', (SELECT MAX(id) FROM aircraft_families));"
        );
        await context.Database.ExecuteSqlRawAsync(
            "SELECT setval('aircraft_models_id_seq', (SELECT MAX(id) FROM aircraft_models));"
        );
        await context.Database.ExecuteSqlRawAsync(
            "SELECT setval('flights_id_seq', (SELECT MAX(id) FROM flights));"
        );
        await context.Database.ExecuteSqlRawAsync(
            "SELECT setval('passengers_id_seq', (SELECT MAX(id) FROM passengers));"
        );
        await context.Database.ExecuteSqlRawAsync(
            "SELECT setval('tickets_id_seq', (SELECT MAX(id) FROM tickets));"
        );
    }
}

/// <summary>
/// Регистрация health-check endpoint'ов для Aspire.
/// </summary>
app.MapDefaultEndpoints();

/// <summary>
/// Подключение Swagger UI (всегда включено для лабораторной работы).
/// </summary>
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();