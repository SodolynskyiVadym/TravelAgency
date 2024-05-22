using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.Implementations;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly HotelRepository _hotelRepository;
    public HotelController(MyDbContext context, IMapper mapper)
    {
        _hotelRepository = new HotelRepository(context, mapper);
    }
    
    [HttpGet("hotel/{id}")]
    public async Task<Hotel?> GetHotel(int id)
    {
        return await _hotelRepository.GetHotelByIdAsync(id);
    }
    
    [HttpGet("getAllHotels")]
    public async Task<List<Hotel>> GetAllHotels()
    {
        return await _hotelRepository.GetAllHotelsListAsync();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> AddHotel(HotelCreateDto hotel)
    {
        await _hotelRepository.AddHotelAsync(hotel);
        return Ok(hotel);
    }
}