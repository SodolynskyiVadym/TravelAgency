using AutoMapper;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PlaceDto, Place>();
        CreateMap<Place, PlaceInfoDto>();
        CreateMap<HotelDto, Hotel>();
        CreateMap<DestinationDto, Destination>();
        CreateMap<TransportDto, Transport>();
        CreateMap<TourDto, Tour>();
        CreateMap<Tour, TourForeignKeyDto>();
        CreateMap<UserDto, User>();
    }
}