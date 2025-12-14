using System.Threading;
using AviaCompany.Application;
using AviaCompany.Application.Contracts;
using AviaCompany.Application.Services;
using AviaCompany.Domain;
using AviaCompany.Infrastructure;
using AviaCompany.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Aspire integration
builder.AddServiceDefaults();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Controllers
builder.Services.AddControllers();

// PostgreSQL via Aspire
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(AviaCompanyProfile));

// DataSeeder as singleton
builder.Services.AddSingleton<DataSeeder>();

// Repositories
builder.Services.AddScoped<IRepository<AircraftFamily, int>, AircraftFamilyRepository>();
builder.Services.AddScoped<IRepository<AircraftModel, int>, AircraftModelRepository>();
builder.Services.AddScoped<IRepository<Flight, int>, FlightRepository>();
builder.Services.AddScoped<IRepository<Passenger, int>, PassengerRepository>();
builder.Services.AddScoped<IRepository<Ticket, int>, TicketRepository>();

// CRUD Services
builder.Services.AddScoped<IApplicationService<FlightDto, FlightCreateUpdateDto, int>, FlightService>();
builder.Services.AddScoped<IApplicationService<AircraftFamilyDto, AircraftFamilyCreateUpdateDto, int>, AircraftFamilyService>();
builder.Services.AddScoped<IApplicationService<AircraftModelDto, AircraftModelCreateUpdateDto, int>, AircraftModelService>();
builder.Services.AddScoped<IApplicationService<PassengerDto, PassengerCreateUpdateDto, int>, PassengerService>();
builder.Services.AddScoped<IApplicationService<TicketDto, TicketCreateUpdateDto, int>, TicketService>();

// Analytics
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

// === Применяем миграции с retry ===
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
            context.Database.Migrate(); // Создаёт таблицы, если их нет
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

    // Заполняем данными, только если БД пустая
    if (!context.AircraftFamilies.Any())
    {
        context.AircraftFamilies.AddRange(seeder.Families);
        context.AircraftModels.AddRange(seeder.Models);
        context.Flights.AddRange(seeder.Flights);
        context.Passengers.AddRange(seeder.Passengers);
        context.Tickets.AddRange(seeder.Tickets);
        context.SaveChanges();
    }
}

// Aspire health endpoints
app.MapDefaultEndpoints();

// Swagger (всегда включён для лабораторной)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();