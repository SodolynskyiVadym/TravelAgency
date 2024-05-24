using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.Implementations;

namespace TravelAgencyAPI.Repositories;

public class TransportRepository : IRepository<Transport, TransportDto>
{
    private readonly MyDbContext _context;
    private readonly IMapper _mapper;

    public TransportRepository(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Transport?> GetByIdAsync(int id)
    {
        return await _context.Transports.FindAsync(id);
    }
    
    public async Task<List<Transport>> GetAllAsync()
    {
        return await _context.Transports.ToListAsync();
    }
    
    public async Task<bool> AddAsync(TransportDto transport)
    {
        await _context.Transports.AddAsync(_mapper.Map<Transport>(transport));
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateAsync(int id, TransportDto transportUpdate)
    {
        Transport? transport = await _context.Transports.FindAsync(id);
        if (transport == null) return false;
        
        transport.Name = transportUpdate.Name ?? transport.Name;
        transport.Description = transportUpdate.Description ?? transport.Description;
        transport.PricePerSeatPerKm = transportUpdate.PricePerSeatPerKm;
        transport.QuantitySeats = transportUpdate.QuantitySeats;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Transport? transport = await _context.Transports.FindAsync(id);
        if(transport == null) return false;
        
        _context.Transports.Remove(transport);
        await _context.SaveChangesAsync();
        return true;
    }
}