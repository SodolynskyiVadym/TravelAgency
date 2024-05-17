using AutoMapper;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PlaceCreateDto, Place>();
        CreateMap<HotelCreateDto, Hotel>();
    }
}