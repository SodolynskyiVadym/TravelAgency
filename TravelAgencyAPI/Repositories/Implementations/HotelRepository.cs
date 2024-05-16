using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public class HotelRepository : IHotelRepository
{
    private MyDbContext _context;
    public HotelRepository(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddHotelAsync(Hotel hotel)
    {
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();
        return true;
    } 
}