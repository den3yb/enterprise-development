// AviaCompany.Application/AviaCompanyProfile.cs

using AutoMapper;
using AviaCompany.Application.Contracts;
using AviaCompany.Domain;

namespace AviaCompany.Application;

/// <summary>
/// Профиль AutoMapper для авиакомпании, для маппинга Entity и Dto сущностей
/// </summary>
public class AviaCompanyProfile : Profile
{
    public AviaCompanyProfile()
    {
        /// <summary>
        /// Для семейства самолетов
        /// </summary>
        CreateMap<AircraftFamily, AircraftFamilyDto>();
        CreateMap<AircraftFamilyCreateUpdateDto, AircraftFamily>();

        /// <summary>
        /// Для моделей
        /// </summary>
        CreateMap<AircraftModel, AircraftModelDto>();
        CreateMap<AircraftModelCreateUpdateDto, AircraftModel>();
        
        /// <summary>
        /// Для Полётов
        /// </summary>
        CreateMap<Flight, FlightDto>();
        CreateMap<FlightCreateUpdateDto, Flight>();

        /// <summary>
        /// Для пасажиров
        /// </summary>
        CreateMap<Passenger, PassengerDto>();
        CreateMap<PassengerCreateUpdateDto, Passenger>();

        /// <summary>
        /// Для билетов
        /// </summary>
        CreateMap<Ticket, TicketDto>();
        CreateMap<TicketCreateUpdateDto, Ticket>();
    }
}