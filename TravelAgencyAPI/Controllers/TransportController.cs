using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class TransportController : ControllerBase
{
    private readonly TransportRepository _transportRepository;
    
    public TransportController(MyDbContext context, IMapper mapper)
    {
        _transportRepository = new TransportRepository(context, mapper);
    }
    
    [HttpGet("{id}")]
    public async Task<Transport?> GetTransport(int id)
    {
        return await _transportRepository.GetByIdAsync(id);
    }
    
    [HttpGet("getAllTransports")]
    public async Task<List<Transport>> GetAllTransports()
    {
        return await _transportRepository.GetAllAsync();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> AddTransport(TransportDto transport)
    {
        await _transportRepository.AddAsync(transport);
        return Ok(transport);
    }
    
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateTransport(int id, TransportDto transport)
    {
        if (await _transportRepository.UpdateAsync(id, transport)) return Ok();
        return NoContent();
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTransport(int id)
    {
        if (await _transportRepository.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}