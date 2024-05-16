using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Controllers;

public class HotelController
{
    private HotelRepository _hotelRepository;
    public HotelController(MyDbContext context)
    {
        _hotelRepository = new HotelRepository(context);
    }
    
    [HttpPost]
    public async Task<Hotel> AddHotel(Hotel hotel)
    {
        await _hotelRepository.AddHotelAsync(hotel);
        return hotel;
    }
}