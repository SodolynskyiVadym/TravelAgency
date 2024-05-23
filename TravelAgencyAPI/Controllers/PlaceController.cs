﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaceController : ControllerBase
{
    private readonly PlaceRepository _placeRepository;

    public PlaceController(MyDbContext context, IMapper mapper)
    {
        _placeRepository = new PlaceRepository(context, mapper);
    }

    [HttpGet("{id}")]
    public async Task<Place?> GetPlace(int id)
    {
        return await _placeRepository.GetByIdAsync(id);
    }
    
    [HttpGet("getAllPlaces")]
    public async Task<List<Place>> GetAllPlaces()
    {
        return await _placeRepository.GetAllAsync();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlace(PlaceCreateDto place)
    {
        await _placeRepository.AddAsync(place);
        return Ok(place);
    }
    
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdatePlace(int id, PlaceCreateDto place)
    {
        if (await _placeRepository.UpdateAsync(id, place)) return Ok();
        return NoContent();
    }
}