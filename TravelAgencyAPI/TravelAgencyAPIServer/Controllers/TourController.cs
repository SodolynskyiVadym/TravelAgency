using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services;

namespace TravelAgencyAPIServer.Controllers;

/// <summary>
/// The TourController class handles tour-related operations for the Travel Agency API.
/// </summary>
[ApiController]
[Route("[controller]")]
public class TourController : ControllerBase
{
    private readonly TourService _tourService;
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the TourController class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mapper">The object mapper.</param>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public TourController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _context = context;
        _tourService = new TourService(context, mapper, redis);
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a tour by its ID.
    /// </summary>
    /// <param name="id">The ID of the tour.</param>
    /// <returns>The tour details.</returns>
    [HttpGet("{id}")]
    public async Task<Tour?> GetTour(int id)
    {
        return await _tourService.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves all tours.
    /// </summary>
    /// <returns>A list of all tours.</returns>
    [HttpGet("getAllTours")]
    public async Task<List<Tour>> GetAllTours()
    {
        return await _tourService.GetAllAsync();
    }

    /// <summary>
    /// Retrieves all available tours.
    /// </summary>
    /// <returns>A list of available tours.</returns>
    [HttpGet("getAvailableTours")]
    public async Task<List<TourBasicInfoDto>> GetAvailableTours()
    {
        return await _tourService.GetAvailableTours();
    }

    /// <summary>
    /// Retrieves all unavailable tours.
    /// </summary>
    /// <returns>A list of unavailable tours.</returns>
    [HttpGet("getUnavailableTours")]
    public async Task<List<Tour>> GetUnavailableTours()
    {
        return await _tourService.GetUnavailableTours();
    }
    

    /// <summary>
    /// Adds a new tour.
    /// </summary>
    /// <param name="tour">The tour details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddTour(TourDto tour)
    {
        await _tourService.AddAsync(tour);
        return Ok(tour);
    }

    /// <summary>
    /// Updates an existing tour.
    /// </summary>
    /// <param name="tour">The updated tour details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateTour(TourDto tour)
    {
        if (await _tourService.UpdateAsync(tour)) return Ok();
        return NoContent();
    }

    /// <summary>
    /// Deletes a tour by its ID.
    /// </summary>
    /// <param name="id">The ID of the tour.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTour(int id)
    {
        if (await _tourService.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}