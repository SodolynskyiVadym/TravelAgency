using System.Collections;
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
/// The PlaceController class handles place-related operations for the Travel Agency API.
/// </summary>
[ApiController]
[Route("[controller]")]
public class PlaceController : ControllerBase
{
    private readonly PlaceService _placeService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the PlaceController class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mapper">The object mapper.</param>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public PlaceController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _mapper = mapper;
        _placeService = new PlaceService(context, mapper, redis);
    }

    /// <summary>
    /// Retrieves a place by its ID.
    /// </summary>
    /// <param name="id">The ID of the place.</param>
    /// <returns>The place details.</returns>
    [HttpGet("{id}")]
    public async Task<PlaceDto?> GetPlace(int id)
    {
        Place? place = await _placeService.GetByIdAsync(id);
        return place == null ? null : _mapper.Map<PlaceDto>(place);
    }

    /// <summary>
    /// Retrieves all places.
    /// </summary>
    /// <returns>A list of all places.</returns>
    [HttpGet("getAllPlaces")]
    public async Task<List<Place>> GetAllPlaces()
    {
        return await _placeService.GetAllAsync();
    }

    /// <summary>
    /// Retrieves all places with images.
    /// </summary>
    /// <returns>A list of all places with images.</returns>
    [HttpGet("getPlacesWithImages")]
    public async Task<IEnumerable<Place>> GetPlacesWithImages()
    {
        return await _placeService.GetAllAsync();
    }

    /// <summary>
    /// Retrieves information about all places.
    /// </summary>
    /// <param name="id">The ID of the place.</param>
    /// <returns>A list of place information DTOs.</returns>
    [HttpGet("getPlacesInfo")]
    public async Task<IEnumerable<PlaceInfoDto>> GetPlaceInfo(int id)
    {
        return await _placeService.GetPlacesInfo();
    }

    /// <summary>
    /// Creates a new place.
    /// </summary>
    /// <param name="place">The place details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlace(PlaceDto place)
    {
        await _placeService.AddAsync(place);
        return Ok(place);
    }

    /// <summary>
    /// Updates an existing place.
    /// </summary>
    /// <param name="place">The updated place details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdatePlace(PlaceDto place)
    {
        if (await _placeService.UpdateAsync(place)) return Ok();
        return NoContent();
    }
}