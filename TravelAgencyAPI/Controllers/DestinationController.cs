using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DestinationController : ControllerBase
{
    private readonly DestinationRepository _destinationRepository;
    
    public DestinationController(MyDbContext context, IMapper mapper)
    {
        _destinationRepository = new DestinationRepository(context, mapper);
    }
    
    [HttpGet("{id}")]
    public async Task<Destination?> GetDestination(int id)
    {
        return await _destinationRepository.GetByIdAsync(id);
    }
    
    [HttpGet("getAllDestinations")]
    public async Task<List<Destination>> GetAllDestinations()
    {
        return await _destinationRepository.GetAllAsync();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> AddDestination(DestinationDto destination)
    {
        await _destinationRepository.AddAsync(destination);
        return Ok(destination);
    }
    
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateDestination(int id, DestinationDto destination)
    {
        if (await _destinationRepository.UpdateAsync(id, destination)) return Ok();
        return NoContent();
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteDestination(int id)
    {
        if (await _destinationRepository.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}