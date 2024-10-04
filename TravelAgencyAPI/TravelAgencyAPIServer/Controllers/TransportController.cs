using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services;

namespace TravelAgencyAPIServer.Controllers;

/// <summary>
/// The TransportController class handles transport-related operations for the Travel Agency API.
/// </summary>
[ApiController]
[Route("[controller]")]
public class TransportController : ControllerBase
{
    private readonly TransportService _transportService;

    /// <summary>
    /// Initializes a new instance of the TransportController class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mapper">The object mapper.</param>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public TransportController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _transportService = new TransportService(context, mapper, redis);
    }

    /// <summary>
    /// Retrieves transport by its ID.
    /// </summary>
    /// <param name="id">The ID of the transport.</param>
    /// <returns>The transport details.</returns>
    [HttpGet("{id}")]
    public async Task<Transport?> GetTransport(int id)
    {
        return await _transportService.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves all transports.
    /// </summary>
    /// <returns>A list of all transports.</returns>
    [HttpGet("getAllTransports")]
    public async Task<List<Transport>> GetAllTransports()
    {
        return await _transportService.GetAllAsync();
    }

    /// <summary>
    /// Adds a new transport.
    /// </summary>
    /// <param name="transport">The transport details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddTransport(TransportDto transport)
    {
        await _transportService.AddAsync(transport);
        return Ok(transport);
    }

    /// <summary>
    /// Updates an existing transport.
    /// </summary>
    /// <param name="transport">The updated transport details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateTransport(TransportDto transport)
    {
        if (await _transportService.UpdateAsync(transport)) return Ok();
        return NoContent();
    }

    /// <summary>
    /// Deletes transport by its ID.
    /// </summary>
    /// <param name="id">The ID of the transport.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTransport(int id)
    {
        if (await _transportService.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}