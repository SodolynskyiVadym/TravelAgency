using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories.Implementations;

public class PlaceRepository : IPlaceRepository
{
    private readonly MyDbContext _context;
    private readonly IMapper _mapper;

    public PlaceRepository(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Place?> GetAsyncById(int id)
    {
        return await _context.Places
            .Include(place => place.Hotels)
            .FirstOrDefaultAsync(place => place.Id == id);
    }

    public async Task<bool> AddPlaceAsync(PlaceCreateDto place)
    {
        await _context.Places.AddAsync(_mapper.Map<Place>(place));
        await _context.SaveChangesAsync();
        return true;
    }

    private IQueryable<Place> GetAllPlaces()
    {
        return _context.Places
            .Include(place => place.Hotels!);
    }
}