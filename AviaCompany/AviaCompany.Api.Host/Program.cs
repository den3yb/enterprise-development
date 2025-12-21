/// <summary>
/// Точка входа веб-приложения авиакомпании с настройкой зависимостей, миграций и middleware.
/// </summary>
using System.Threading;
using AviaCompany.Application;
using AviaCompany.Application.Contracts;
using AviaCompany.Application.Services;
using AviaCompany.Domain;
using AviaCompany.Infrastructure;
using AviaCompany.Infrastructure.Kafka.Deserializers;
using AviaCompany.Infrastructure.Kafka.Services;
using AviaCompany.Infrastructure.Repositories;
using Confluent.Kafka;
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

/// <summary>
/// Регистрация Кафки.
/// </summary>
builder.Services.AddSingleton<IConsumer<Guid, IList<FlightCreateUpdateDto>>>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var bootstrapServers = (configuration["KAFKA_BOOTSTRAP_SERVERS"] ?? "localhost:9092")
        .Replace("tcp://", "");
    
    var config = new ConsumerConfig
    {
        BootstrapServers = bootstrapServers,
        GroupId = "flight-consumer-group",
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = true
    };

    return new ConsumerBuilder<Guid, IList<FlightCreateUpdateDto>>(config)
        .SetKeyDeserializer(new FlightKeyDeserializer())
        .SetValueDeserializer(new FlightValueDeserializer())
        .Build();
});

builder.Services.AddHostedService<FlightKafkaConsumer>();

var app = builder.Build();

/// <summary>
/// Применение миграций базы данных и тестовых данных.
/// </summary>
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

    await context.Database.MigrateAsync(); 
    await DbSeeder.SeedAllAsync(context);
}

/// <summary>
/// Регистрация health-check endpoint'ов для Aspire.
/// </summary>
app.MapDefaultEndpoints();

/// <summary>
/// Подключение Swagger UI (включен только в режиме Debug-режиме).
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();