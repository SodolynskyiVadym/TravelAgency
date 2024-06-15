using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    
    public TourController(TravelDbContext context, IMapper mapper)
    {
        _tourRepository = new TourRepository(context, mapper);
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
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateTour(int id, TourDto tour)
    {
        if (await _tourRepository.UpdateAsync(id, tour)) return Ok();
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