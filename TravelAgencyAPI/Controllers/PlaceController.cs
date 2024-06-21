using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaceController : ControllerBase
{
    private readonly PlaceRepository _placeRepository;

    public PlaceController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _placeRepository = new PlaceRepository(context, mapper, redis);
    }

    [HttpGet("{id}")]
    public async Task<Place?> GetPlace(int id)
    {
        return await _placeRepository.GetByIdAsync(id);
    }
    
    [HttpGet("getAllPlaces")]
    public async Task<List<Place>> GetAllPlaces()
    {
        return await _placeRepository.GetAllAsync();
    }
    
    [HttpGet("getPlacesWithImages")]
    public async Task<IEnumerable<Place>> GetPlacesWithImages()
    {
        return await _placeRepository.GetAllAsync();
    }
    
    [HttpGet("getPlacesInfo")]
    public async Task<IEnumerable<PlaceInfoDto>> GetPlaceInfo(int id)
    {
        return await _placeRepository.GetPlacesInfo();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlace(PlaceDto place)
    {
        await _placeRepository.AddAsync(place);
        return Ok(place);
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdatePlace(PlaceDto place)
    {
        if (await _placeRepository.UpdateAsync(place)) return Ok();
        return NoContent();
    }
}