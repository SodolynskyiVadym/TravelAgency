﻿using AutoMapper;
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
public class HotelController : ControllerBase
{
    private readonly HotelService _hotelService;
    public HotelController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _hotelService = new HotelService(context, mapper, redis);
    }
    
    
    [HttpGet("{id}")]
    public async Task<Hotel?> GetHotel(int id)
    {
        return await _hotelService.GetByIdAsync(id);
    }
    
    
    [HttpGet("getAllHotels")]
    public async Task<List<Hotel>> GetAllHotels()
    {
        return await _hotelService.GetAllAsync();
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPost("create")]
    public async Task<IActionResult> AddHotel(HotelDto hotel)
    {
        await _hotelService.AddAsync(hotel);
        return Ok(hotel);
    }
    
    
    [Authorize(Roles = "EDITOR, ADMIN")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateHotel(HotelDto hotel)
    {
        if (await _hotelService.UpdateAsync(hotel)) return Ok();
        // Replace NoContent
        return NoContent();
    }
}