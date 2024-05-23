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
    
    [HttpGet("{id}")]
    public async Task<Hotel?> GetHotel(int id)
    {
        return await _hotelRepository.GetByIdAsync(id);
    }
    
    [HttpGet("getAllHotels")]
    public async Task<List<Hotel>> GetAllHotels()
    {
        return await _hotelRepository.GetAllAsync();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> AddHotel(HotelCreateDto hotel)
    {
        await _hotelRepository.AddAsync(hotel);
        return Ok(hotel);
    }
    
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateHotel(int id, HotelCreateDto hotel)
    {
        if (await _hotelRepository.UpdateAsync(id, hotel)) return Ok();
        // Replace NoContent
        return NoContent();
    }
}