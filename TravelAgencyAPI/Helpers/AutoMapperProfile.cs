using AutoMapper;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PlaceDto, Place>();
        CreateMap<HotelDto, Hotel>();
        CreateMap<DestinationDto, Destination>();
        CreateMap<TransportDto, Transport>();
        CreateMap<TourDto, Tour>();
    }
}