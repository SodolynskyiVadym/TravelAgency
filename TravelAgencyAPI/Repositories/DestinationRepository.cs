using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories;

public class DestinationRepository : IRepository<Destination, DestinationDto>
{
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;
    
    public DestinationRepository(TravelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Destination?> GetByIdAsync(int id)
    {
        return await _context.Destinations.FindAsync(id);
    }
    
    public async Task<List<Destination>> GetAllAsync()
    {
        return await _context.Destinations.ToListAsync();
    }
    
    public async Task<bool> AddAsync(DestinationDto destination)
    {
        await _context.Destinations.AddAsync(_mapper.Map<Destination>(destination));
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateAsync(int id, DestinationDto destinationUpdate)
    {
        Destination? destination = await _context.Destinations.FindAsync(id);
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