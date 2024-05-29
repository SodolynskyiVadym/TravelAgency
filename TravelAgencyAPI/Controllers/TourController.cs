using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class TourController : ControllerBase
{
    private readonly TourRepository _tourRepository;
    
    public TourController(TravelDbContext context, IMapper mapper)
    {
        _tourRepository = new TourRepository(context, mapper);
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
    
    [HttpPost("create")]
    public async Task<IActionResult> AddTour(TourDto tour)
    {
        await _tourRepository.AddAsync(tour);
        return Ok(tour);
    }
    
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateTour(int id, TourDto tour)
    {
        if (await _tourRepository.UpdateAsync(id, tour)) return Ok();
        return NoContent();
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTour(int id)
    {
        if (await _tourRepository.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}