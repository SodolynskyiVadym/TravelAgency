using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.Implementations;

namespace TravelAgencyAPI.Repositories;

public class HotelRepository : IRepository<Hotel, HotelDto>
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

    public async Task<bool> AddAsync(HotelDto hotel)
    {
        await _context.Hotels.AddAsync(_mapper.Map<Hotel>(hotel));
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, HotelDto hotelUpdate)
    {
        Hotel? hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null) return false;
        
        hotel.Name = hotelUpdate.Name ?? hotel.Name ;
        hotel.Address = hotelUpdate.Address ?? hotel.Address;
        hotel.Description = hotelUpdate.Description ?? hotel.Description;
        hotel.PricePerNight = hotelUpdate.PricePerNight;
        hotel.PlaceId = hotelUpdate.PlaceId != 0 ? hotelUpdate.PlaceId : hotel.PlaceId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Hotel? hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null) return false;

        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();

        return true;
    }
}