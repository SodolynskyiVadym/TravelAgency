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
    private readonly TransportService _transportService;
    
    public TransportController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _transportService = new TransportService(context, mapper, redis);
    }
    
    
    [HttpGet("{id}")]
    public async Task<Transport?> GetTransport(int id)
    {
        return await _transportService.GetByIdAsync(id);
    }
    
    [HttpGet("getAllTransports")]
    public async Task<List<Transport>> GetAllTransports()
    {
        return await _transportService.GetAllAsync();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddTransport(TransportDto transport)
    {
        await _transportService.AddAsync(transport);
        return Ok(transport);
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateTransport(TransportDto transport)
    {
        if (await _transportService.UpdateAsync(transport)) return Ok();
        return NoContent();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTransport(int id)
    {
        if (await _transportService.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}