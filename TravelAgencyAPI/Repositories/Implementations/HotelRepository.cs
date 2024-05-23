﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories.Implementations;

public class HotelRepository : IRepository<Hotel, HotelCreateDto>
{
    private MyDbContext _context;
    private readonly IMapper _mapper;
    public HotelRepository(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    
    public async Task<Hotel?> GetByIdAsync(int id)
    {
        return await _context.Hotels
            .Include(h => h.Place)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<List<Hotel>> GetAllAsync()
    {
        return await _context.Hotels.ToListAsync();
    }

    public async Task<bool> AddAsync(HotelCreateDto hotel)
    {
        await _context.Hotels.AddAsync(_mapper.Map<Hotel>(hotel));
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, HotelCreateDto hotelUpdate)
    {
        Hotel? hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null) return false;
        
        hotel.Name = hotelUpdate.Name ?? hotel.Name ;
        hotel.Address = hotelUpdate.Address ?? hotel.Address;
        hotel.Description = hotelUpdate.Description ?? hotel.Description;
        hotel.PricePerNight = hotelUpdate.PricePerNight;

        await _context.SaveChangesAsync();
        return true;
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}