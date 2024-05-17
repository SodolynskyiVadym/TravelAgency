using AutoMapper;
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
    
    public async Task<bool> AddHotelAsync(HotelCreateDto hotel)
    {
        await _context.Hotels.AddAsync(_mapper.Map<Hotel>(hotel));
        await _context.SaveChangesAsync();
        return true;
    } 
}