using AutoMapper;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Models;

namespace TravelAgencyAPIServer.Helpers;

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
        CreateMap<(Tour, string, int), TourEmailDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item1.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Item1.Description))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Item1.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Item1.EndDate))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Item1.Price))
            .ForMember(dest => dest.PurchasedSeats, opt => opt.MapFrom(src => src.Item3))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Item1.ImageUrl))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Item2));
        CreateMap<Tour, TourBasicInfoDto>()
            .ForMember(dest => dest.DestinationsNames, opt => 
                    opt.MapFrom(src => src.Destinations.Select(d => d.Hotel.Place.Name).ToList()));
        CreateMap<UserDto, User>();
        CreateMap<User, UserEmailRoleDto>();
        CreateMap<(UserEmailRoleDto, string), UserEmailRolePasswordDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Item1.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Item1.Role))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Item2));
        CreateMap<PaymentDto, Payment>();
        CreateMap<Payment, PaymentDto>();
        CreateMap<Review, ReviewDto>();
        CreateMap<ReviewDto, Review>();
    }
}