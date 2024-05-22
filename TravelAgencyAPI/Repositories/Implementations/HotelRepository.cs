using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories.Implementations;

public class HotelRepository : IHotelRepository
{
    private MyDbContext _context;
    private readonly IMapper _mapper;
    public HotelRepository(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Hotel?> GetHotelByIdAsync(int id)
    {
        return _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<List<Hotel>> GetAllHotelsListAsync()
    {
        return await _context.Hotels.ToListAsync();
    }

    public async Task<bool> AddHotelAsync(HotelCreateDto hotel)
    {
        await _context.Hotels.AddAsync(_mapper.Map<Hotel>(hotel));
        await _context.SaveChangesAsync();
        return true;
    } 
}