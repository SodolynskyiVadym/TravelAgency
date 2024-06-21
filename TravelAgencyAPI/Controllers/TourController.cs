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
    private readonly TourRepository _tourRepository;
    private readonly IMapper _mapper;
    
    public TourController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _tourRepository = new TourRepository(context, mapper, redis);
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    public async Task<Tour?> GetTour(int id)
    {
        return await _tourRepository.GetByIdAsync(id);
    }
    
    [HttpGet("getAllTours")]
    public async Task<List<Tour>> GetAllTours()
    {
        return await _tourRepository.GetAllAsync();
    }
    
    
    [HttpGet("getToursForeignKeys")]
    public async Task<IEnumerable<TourForeignKeyDto>> GetToursWithDestinations()
    {
        IEnumerable<Tour> tours = await _tourRepository.GetAllAsync();
        return tours.Select(tour => _mapper.Map<TourForeignKeyDto>(tour));
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddTour(TourDto tour)
    {
        await _tourRepository.AddAsync(tour);
        return Ok(tour);
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateTour(TourDto tour)
    {
        if (await _tourRepository.UpdateAsync(tour)) return Ok();
        return NoContent();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTour(int id)
    {
        if (await _tourRepository.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}