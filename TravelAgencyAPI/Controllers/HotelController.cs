using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly HotelRepository _hotelRepository;
    public HotelController(TravelDbContext context, IMapper mapper)
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
    public async Task<IActionResult> AddHotel(HotelDto hotel)
    {
        await _hotelRepository.AddAsync(hotel);
        return Ok(hotel);
    }
    
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateHotel(int id, HotelDto hotel)
    {
        if (await _hotelRepository.UpdateAsync(id, hotel)) return Ok();
        // Replace NoContent
        return NoContent();
    }
}