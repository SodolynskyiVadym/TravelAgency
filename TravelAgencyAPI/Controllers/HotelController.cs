using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
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
    
    [HttpPost("create")]
    public async Task<IActionResult> AddHotel(HotelCreateDto hotel)
    {
        await _hotelRepository.AddHotelAsync(hotel);
        return Ok(hotel);
    }
}