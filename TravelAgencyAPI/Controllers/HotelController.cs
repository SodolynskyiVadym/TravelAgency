using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly HotelRepository _hotelRepository;
    public HotelController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _hotelRepository = new HotelRepository(context, mapper, redis);
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
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddHotel(HotelDto hotel)
    {
        await _hotelRepository.AddAsync(hotel);
        return Ok(hotel);
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateHotel(int id, HotelDto hotel)
    {
        if (await _hotelRepository.UpdateAsync(id, hotel)) return Ok();
        // Replace NoContent
        return NoContent();
    }
}