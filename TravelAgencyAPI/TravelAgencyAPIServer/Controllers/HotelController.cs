using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services;

namespace TravelAgencyAPIServer.Controllers;

/// <summary>
/// The HotelController class handles hotel-related operations for the Travel Agency API.
/// </summary>
[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly HotelService _hotelService;

    /// <summary>
    /// Initializes a new instance of the HotelController class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mapper">The object mapper.</param>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public HotelController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _hotelService = new HotelService(context, mapper, redis);
    }

    /// <summary>
    /// Retrieves a hotel by its ID.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <returns>The hotel details.</returns>
    [HttpGet("{id}")]
    public async Task<Hotel?> GetHotel(int id)
    {
        return await _hotelService.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves all hotels.
    /// </summary>
    /// <returns>A list of all hotels.</returns>
    [HttpGet("getAllHotels")]
    public async Task<List<Hotel>> GetAllHotels()
    {
        return await _hotelService.GetAllAsync();
    }

    /// <summary>
    /// Adds a new hotel.
    /// </summary>
    /// <param name="hotel">The hotel details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddHotel(HotelDto hotel)
    {
        await _hotelService.AddAsync(hotel);
        return Ok(hotel);
    }

    /// <summary>
    /// Updates an existing hotel.
    /// </summary>
    /// <param name="hotel">The updated hotel details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateHotel(HotelDto hotel)
    {
        if (await _hotelService.UpdateAsync(hotel)) return Ok();
        return BadRequest();
    }
}