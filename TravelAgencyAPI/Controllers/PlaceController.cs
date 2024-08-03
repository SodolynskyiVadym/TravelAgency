using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;
using TravelAgencyAPI.Services;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaceController : ControllerBase
{
    private readonly PlaceService _placeService;

    public PlaceController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _placeService = new PlaceService(context, mapper, redis);
    }

    [HttpGet("{id}")]
    public async Task<Place?> GetPlace(int id)
    {
        return await _placeService.GetByIdAsync(id);
    }
    
    [HttpGet("getAllPlaces")]
    public async Task<List<Place>> GetAllPlaces()
    {
        return await _placeService.GetAllAsync();
    }
    
    [HttpGet("getPlacesWithImages")]
    public async Task<IEnumerable<Place>> GetPlacesWithImages()
    {
        return await _placeService.GetAllAsync();
    }
    
    [HttpGet("getPlacesInfo")]
    public async Task<IEnumerable<PlaceInfoDto>> GetPlaceInfo(int id)
    {
        return await _placeService.GetPlacesInfo();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlace(PlaceDto place)
    {
        if(await _placeService.IsUsedUniqueAttributes(place)) return BadRequest("Place already exists!");
        await _placeService.AddAsync(place);
        return Ok(place);
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdatePlace(PlaceDto place)
    {
        if(await _placeService.IsUsedUniqueAttributes(place)) return BadRequest("Place already exists!");
        if (await _placeService.UpdateAsync(place)) return Ok();
        return NoContent();
    }
}