using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DestinationController : ControllerBase
{
    private readonly TravelDbContext _context;
    
    public DestinationController(TravelDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("getAllDestinations")]
    public async Task<List<Destination>> GetAllDestinations()
    {
        return await _context.Destinations.ToListAsync();
    }
}