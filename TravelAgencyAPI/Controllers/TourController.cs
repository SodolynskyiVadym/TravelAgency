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
public class TourController : ControllerBase
{
    private readonly TourService _tourService;
    private readonly IMapper _mapper;
    
    public TourController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
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
    public async Task<List<Tour>> GetAvailableTours()
    {
        return await _tourService.GetAvailableTours();
    }
    
    [HttpGet("getUnavailableTours")]
    public async Task<List<Tour>> GetUnavailableTours()
    {
        return await _tourService.GetUnavailableTours();
    }
    
    
    [HttpGet("getToursForeignKeys")]
    public async Task<IEnumerable<TourForeignKeyDto>> GetToursWithDestinations()
    {
        IEnumerable<Tour> tours = await _tourService.GetAllAsync();
        return tours.Select(tour => _mapper.Map<TourForeignKeyDto>(tour));
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