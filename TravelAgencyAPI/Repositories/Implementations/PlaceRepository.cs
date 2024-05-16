using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories.Implementations;

public class PlaceRepository : IPlaceRepository
{
    private MyDbContext _context;

    public PlaceRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddPlaceAsync(Place place)
    {
        await _context.Places.AddAsync(place);
        await _context.SaveChangesAsync();
        return true;
    }
}