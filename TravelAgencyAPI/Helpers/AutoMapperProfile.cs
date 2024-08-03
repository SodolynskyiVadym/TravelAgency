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
        CreateMap<Tour, TourBasicInfoDto>()
            .ForMember(dest => dest.DestinationsNames, opt => 
                    opt.MapFrom(src => src.Destinations.Select(d => d.Hotel.Place.Name).ToList()));
        CreateMap<UserDto, User>();
        CreateMap<User, UserEmailRoleDto>();
        CreateMap<PaymentDto, Payment>();
        CreateMap<Payment, PaymentDto>();
        CreateMap<Review, ReviewDto>();
        CreateMap<ReviewDto, Review>();
    }
}