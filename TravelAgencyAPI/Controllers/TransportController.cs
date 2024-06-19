using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class TransportController : ControllerBase
{
    private readonly TransportRepository _transportRepository;
    
    public TransportController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _transportRepository = new TransportRepository(context, mapper, redis);
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
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddTransport(TransportDto transport)
    {
        await _transportRepository.AddAsync(transport);
        return Ok(transport);
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateTransport(int id, TransportDto transport)
    {
        if (await _transportRepository.UpdateAsync(id, transport)) return Ok();
        return NoContent();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTransport(int id)
    {
        if (await _transportRepository.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}