using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DestinationController : ControllerBase
{
    private readonly DestinationService _destinationService;
    
    public DestinationController(TravelDbContext context, IMapper mapper)
    {
        _destinationService = new DestinationService(context, mapper);
    }
    
    [HttpGet("{id}")]
    public async Task<Destination?> GetDestination(int id)
    {
        return await _destinationService.GetByIdAsync(id);
    }
    
    [HttpGet("getAllDestinations")]
    public async Task<List<Destination>> GetAllDestinations()
    {
        return await _destinationService.GetAllAsync();
    }
    
    // [HttpPost("createDestinations")]
    // public async Task<IActionResult> UpdateDestinations(IEnumerable<DestinationDto> destinations, int tourId)
    // {
    //     if(await _destinationService.UpdateDestinationsAsync(destinations, tourId)) return Ok(destinations);
    //     return BadRequest();
    // }
    
    // [HttpPatch("update/{id}")]
    // public async Task<IActionResult> UpdateDestination(int id, DestinationDto destination)
    // {
    //     if (await _destinationService.UpdateAsync(id, destination)) return Ok();
    //     return NoContent();
    // }
    
    // [HttpDelete("delete/{id}")]
    // public async Task<IActionResult> DeleteDestination(int id)
    // {
    //     if (await _destinationService.DeleteAsync(id)) return Ok();
    //     return NoContent();
    // }
}