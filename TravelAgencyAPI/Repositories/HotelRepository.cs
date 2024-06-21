using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories;

public class HotelRepository : IRepository<Hotel, HotelDto>
{
    private TravelDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDatabase _redis;
    public HotelRepository(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redisConnection)
    {
        _context = context;
        _mapper = mapper;
        _redis = redisConnection.GetDatabase();
    }
    
    
    public async Task<Hotel?> GetByIdAsync(int id)
    {
        string redisKey = "hotel" + id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            string jsonData = await _redis.StringGetAsync(redisKey);
            return JsonConvert.DeserializeObject<Hotel>(jsonData);
        }
        Hotel? hotel =  await _context.Hotels
            .Include(h => h.Place)
            .FirstOrDefaultAsync(h => h.Id == id);
        if(hotel != null) 
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(hotel));
        return hotel;
    }

    public async Task<List<Hotel>> GetAllAsync()
    {
        return await _context.Hotels
            .Include(h => h.Place)
            .ToListAsync();
    }

    public async Task<int> AddAsync(HotelDto hotelDto)
    {
        Hotel hotel = _mapper.Map<Hotel>(hotelDto);
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();
        return hotel.Id;
    }

    public async Task<bool> UpdateAsync(HotelDto hotelUpdate)
    {
        Hotel? hotel = await _context.Hotels.FindAsync(hotelUpdate.Id);
        if (hotel == null) return false;
        
        string redisKey = "hotel" + hotelUpdate.Id;
        
        hotel.Name = hotelUpdate.Name ?? hotel.Name ;
        hotel.Address = hotelUpdate.Address ?? hotel.Address;
        hotel.Description = hotelUpdate.Description ?? hotel.Description;
        hotel.PricePerNight = hotelUpdate.PricePerNight;
        hotel.PlaceId = hotelUpdate.PlaceId != 0 ? hotelUpdate.PlaceId : hotel.PlaceId;
        hotel.ImageUrl = hotelUpdate.ImageUrl ?? hotel.ImageUrl;

        await _context.SaveChangesAsync();
        if(await _redis.KeyExistsAsync(redisKey))
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(hotel));
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Hotel? hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null) return false;
        
        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();
        await _redis.KeyDeleteAsync("hotel" + id);
        return true;
    }
}