using AviaCompany.Application;
using AviaCompany.Application.Contracts;
using AviaCompany.Application.Contracts.DTOs.AircraftFamily;
using AviaCompany.Application.Contracts.DTOs.AircraftModel;
using AviaCompany.Application.Contracts.DTOs.Flight;
using AviaCompany.Application.Contracts.DTOs.Passenger;
using AviaCompany.Application.Contracts.DTOs.Ticket;
using AviaCompany.Application.Services;
using AviaCompany.Domain;
using AviaCompany.Infrastructure;
using AviaCompany.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AviaCompanyProfile());
});

builder.Services.AddScoped<IRepository<AircraftFamily, int>, AircraftFamilyRepository>();
builder.Services.AddScoped<IRepository<AircraftModel, int>, AircraftModelRepository>();
builder.Services.AddScoped<IRepository<Flight, int>, FlightRepository>();
builder.Services.AddScoped<IRepository<Passenger, int>, PassengerRepository>();
builder.Services.AddScoped<IRepository<Ticket, int>, TicketRepository>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddScoped<IApplicationService<AircraftFamilyDto, AircraftFamilyCreateUpdateDto, int>, AircraftFamilyService>();
builder.Services.AddScoped<IApplicationService<AircraftModelDto, AircraftModelCreateUpdateDto, int>, AircraftModelService>();
builder.Services.AddScoped<IApplicationService<FlightDto, FlightCreateUpdateDto, int>, FlightService>();
builder.Services.AddScoped<IApplicationService<PassengerDto, PassengerCreateUpdateDto, int>, PassengerService>();
builder.Services.AddScoped<IApplicationService<TicketDto, TicketCreateUpdateDto, int>, TicketService>();

builder.AddServiceDefaults();

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.GetName().Name!.StartsWith("AviaCompany"))
        .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            c.IncludeXmlComments(xmlPath);
    }

    c.UseInlineDefinitionsForEnums();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();