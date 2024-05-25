using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.Implementations;

namespace TravelAgencyAPI.Repositories;

public class TourRepository : IRepository<Tour, TourDto>
{
    private MyDbContext _context;
    private readonly IMapper _mapper;

    public TourRepository(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Tour?> GetByIdAsync(int id)
    {
        return await _context.Tours
            .Include(p => p.Destinations)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
    public async Task<List<Tour>> GetAllAsync()
    {
        return await _context.Tours.ToListAsync();
    }
    
    public async Task<bool> AddAsync(TourDto tour)
    {
        await _context.Tours.AddAsync(_mapper.Map<Tour>(tour));
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateAsync(int id, TourDto tourUpdate)
    {
        Tour? tour = await _context.Tours.FindAsync(id);
        if (tour == null) return false;
        
        tour.Name = tourUpdate.Name ?? tour.Name;
        tour.Description = tourUpdate.Description ?? tour.Description;
        tour.Price = tourUpdate.Price;
        tour.QuantitySeats = tourUpdate.QuantitySeats;
        
        await _context.SaveChangesAsync();
        return true;
    }  
    
    public async Task<bool> DeleteAsync(int id)
    {
        Tour? tour = await _context.Tours.FindAsync(id);
        if (tour == null) return false;
        
        _context.Tours.Remove(tour);
        await _context.SaveChangesAsync();
        
        return true;
    }
}