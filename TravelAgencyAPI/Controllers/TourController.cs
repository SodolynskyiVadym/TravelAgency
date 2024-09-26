using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Services;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class TourController : ControllerBase
{
    private readonly TourService _tourService;
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;
    
    public TourController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _context = context;
        _tourService = new TourService(context, mapper, redis);
        _mapper = mapper;
    }
    
    
    [HttpGet("{id}")]
    public async Task<Tour?> GetTour(int id)
    {
        return await _tourService.GetByIdAsync(id);
    }
    
    
    [HttpGet("getAllTours")]
    public async Task<List<Tour>> GetAllTours()
    {
        return await _tourService.GetAllAsync();
    }
    
    
    [HttpGet("getAvailableTours")]
    public async Task<List<TourBasicInfoDto>> GetAvailableTours()
    {
        return await _tourService.GetAvailableTours();
    }
    
    [HttpGet("getUnavailableTours")]
    public async Task<List<Tour>> GetUnavailableTours()
    {
        return await _tourService.GetUnavailableTours();
    }
    
    
    [HttpGet("getTourPlacesId")]
    public async Task<List<int>> GetToursPlacesId()
    {
        List<int> destinationsPlaceId = await _context.Destinations
            .Include(d => d.Hotel)
            .Select(d => d.Hotel.PlaceId)
            .ToListAsync();

        List<int> tourPlacesStartId = await _context.Tours
            .Select(t => t.PlaceStartId)
            .ToListAsync();
        
        
        List<int> tourPlacesEndId = await _context.Tours
            .Select(t => t.PlaceEndId)
            .ToListAsync();

        return destinationsPlaceId.Concat(tourPlacesStartId).Concat(tourPlacesEndId).Distinct().ToList();
    }
    
    [HttpGet("getTourTransportsId")]
    public async Task<List<int>> GetToursTransportsId()
    {
        List<int> destinationsTransportId = await _context.Destinations
            .Select(d => d.TransportId)
            .ToListAsync();

        List<int> tourTransportsId = await _context.Tours
            .Select(t => t.TransportToEndId)
            .ToListAsync();
        
        return destinationsTransportId.Concat(tourTransportsId).Distinct().ToList();
    }
    
    
    [HttpGet("getTourHotelsId")]
    public async Task<List<int>> GetToursHotelsId()
    {
        return await _context.Destinations
            .Select(d => d.HotelId)
            .ToListAsync();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddTour(TourDto tour)
    {
        await _tourService.AddAsync(tour);
        return Ok(tour);
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateTour(TourDto tour)
    {
        if (await _tourService.UpdateAsync(tour)) return Ok();
        return NoContent();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTour(int id)
    {
        if (await _tourService.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}