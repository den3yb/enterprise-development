using Microsoft.EntityFrameworkCore;
using AviaCompany.Domain;

namespace AviaCompany.Infrastructure;


/// <summary>
/// Контекст бд Авиакомпании
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Задания таблиц 
    /// </summary>
    public DbSet<AircraftFamily> AircraftFamilies { get; set; }
    public DbSet<AircraftModel> AircraftModels { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    /// <summary>
    /// Конфигурация модели бд задающая все необходимые поля, связи и параметры  удаления
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AircraftFamily>(builder =>
        {
            builder.ToTable("aircraft_families");

            builder.HasKey(af => af.Id);

            builder.Property(af => af.Id).HasColumnName("id");
            builder.Property(af => af.Name).IsRequired().HasMaxLength(100).HasColumnName("name");
            builder.Property(af => af.Manufacturer).IsRequired().HasMaxLength(100).HasColumnName("manufacturer");
        });

        modelBuilder.Entity<AircraftModel>(builder =>
        {
            builder.ToTable("aircraft_models");

            builder.HasKey(am => am.Id);

            builder.Property(am => am.Id).HasColumnName("id");
            builder.Property(am => am.Name).IsRequired().HasMaxLength(100).HasColumnName("name");
            builder.Property(am => am.FlightRange).HasColumnName("flight_range");
            builder.Property(am => am.PassengerCapacity).HasColumnName("passenger_capacity");
            builder.Property(am => am.CargoCapacity).HasColumnName("cargo_capacity");
            builder.Property(am => am.AircraftFamilyId).HasColumnName("aircraft_family_id");

            builder.HasOne(am => am.AircraftFamily)
                   .WithMany(af => af.Models!)
                   .HasForeignKey(am => am.AircraftFamilyId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Flight>(builder =>
        {
            builder.ToTable("flights");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id).HasColumnName("id");
            builder.Property(f => f.Code).IsRequired().HasMaxLength(20).HasColumnName("code");
            builder.Property(f => f.DeparturePoint).IsRequired().HasMaxLength(100).HasColumnName("departure_point");
            builder.Property(f => f.ArrivalPoint).IsRequired().HasMaxLength(100).HasColumnName("arrival_point");
            builder.Property(f => f.DepartureDate).HasColumnName("departure_date");
            builder.Property(f => f.ArrivalDate).HasColumnName("arrival_date");
            builder.Property(f => f.Duration).HasColumnName("duration");
            builder.Property(f => f.AircraftModelId).HasColumnName("aircraft_model_id");

            builder.HasOne(f => f.AircraftModel)
                   .WithMany(am => am.Flights!)
                   .HasForeignKey(f => f.AircraftModelId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Passenger>(builder =>
        {
            builder.ToTable("passengers");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.PassportNumber).IsRequired().HasMaxLength(20).HasColumnName("passport_number");
            builder.Property(p => p.FullName).IsRequired().HasMaxLength(200).HasColumnName("full_name");
            builder.Property(p => p.BirthDate).HasColumnName("birth_date");
        });

        modelBuilder.Entity<Ticket>(builder =>
        {
            builder.ToTable("tickets");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("id");
            builder.Property(t => t.SeatNumber).IsRequired().HasMaxLength(10).HasColumnName("seat_number");
            builder.Property(t => t.HasHandLuggage).HasColumnName("has_hand_luggage");
            builder.Property(t => t.LuggageWeight).HasColumnName("luggage_weight");
            builder.Property(t => t.FlightId).HasColumnName("flight_id");
            builder.Property(t => t.PassengerId).HasColumnName("passenger_id");

            builder.HasOne(t => t.Flight)
                   .WithMany(f => f.Ticket!)
                   .HasForeignKey(t => t.FlightId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Passenger)
                   .WithMany(p => p.Ticket!)
                   .HasForeignKey(t => t.PassengerId)
                   .OnDelete(DeleteBehavior.Cascade);
            });
    }
}