using AviaCompany.Application.Contracts;
using AviaCompany.Generator.Kafka.Host;
using AviaCompany.Generator.Kafka.Host.Interfaces;
using AviaCompany.Generator.Kafka.Host.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IProducerService, FlightKafkaProducer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();