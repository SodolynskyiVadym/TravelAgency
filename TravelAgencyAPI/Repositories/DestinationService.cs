using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories;

public class DestinationService : IRepository<Destination, DestinationDto>
{
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;
    
    public DestinationService(TravelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Destination?> GetByIdWithIncludeAsync(int id)
    {
        return await _context.Destinations.Include(d => d.Hotel).FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Destination?> GetByIdAsync(int id)
    {
        return await _context.Destinations.FindAsync(id);
    }
    
    public async Task<List<Destination>> GetAllAsync()
    {
        return await _context.Destinations.Include(d => d.Hotel).ToListAsync();
    }
    
    public async Task<int> AddAsync(DestinationDto destinationDto)
    {
        await _context.Destinations.AddAsync(_mapper.Map<Destination>(destinationDto));
        await _context.SaveChangesAsync();
        return 0;
    }
    
    public async Task<bool> UpdateAsync(DestinationDto destinationUpdate)
    {
        Destination? destination = await _context.Destinations.FindAsync(destinationUpdate.Id);
        if (destination == null) return false;
        
        destination.StartDate = destinationUpdate.StartDate ?? destination.StartDate;
        destination.EndDate = destinationUpdate.EndDate ?? destination.StartDate;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        Destination? destination = await _context.Destinations.FindAsync(id);
        if (destination == null) return false;
        
        _context.Destinations.Remove(destination);
        await _context.SaveChangesAsync();
        
        return true;
    }
}