using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.Implementations;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaceController : ControllerBase
{
    private readonly PlaceRepository _placeRepository;

    public PlaceController(MyDbContext context, IMapper mapper)
    {
        _placeRepository = new PlaceRepository(context, mapper);
    }

    [HttpGet("{id}")]
    public async Task<Place?> GetPlace(int id)
    {
        return await _placeRepository.GetAsyncById(id);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlace(PlaceCreateDto place)
    {
        await _placeRepository.AddPlaceAsync(place);
        return Ok(place);
    }
}