using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;
using TravelAgencyAPI.Services;

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
}