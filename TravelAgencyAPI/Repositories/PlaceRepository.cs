using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.Implementations;

namespace TravelAgencyAPI.Repositories;

public class PlaceRepository : IRepository<Place, PlaceCreateDto>
{
    private readonly MyDbContext _context;
    private readonly IMapper _mapper;

    public PlaceRepository(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Place?> GetByIdAsync(int id)
    {
        return await _context.Places.FirstOrDefaultAsync(place => place.Id == id);
    }

    public async Task<List<Place>> GetAllAsync()
    {
        return await _context.Places.ToListAsync();
    }

    public async Task<bool> AddAsync(PlaceCreateDto place)
    {
        await _context.Places.AddAsync(_mapper.Map<Place>(place));
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, PlaceCreateDto entity)
    {
        Place? place = await _context.Places.FindAsync(id);
        if (place == null) return false;
        
        place.Name = entity.Name ?? place.Name;
        place.Description = entity.Description ?? place.Description;
        place.Country = entity.Country ?? place.Country;
        place.SiteUrl = entity.SiteUrl ?? place.SiteUrl;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Place place = await _context.Places.FindAsync(id);
        if (place == null) return false;

        _context.Places.Remove(place);
        await _context.SaveChangesAsync();

        return true;
    }
}