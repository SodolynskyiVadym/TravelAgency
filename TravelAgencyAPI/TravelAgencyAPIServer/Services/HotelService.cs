using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services.Interfaces;

namespace TravelAgencyAPIServer.Services;

public class HotelService : IRepository<Hotel, HotelDto>
{
    private TravelDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDatabase _redis;

    public HotelService(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redisConnection)
    {
        _context = context;
        _mapper = mapper;
        _redis = redisConnection.GetDatabase();
    }


    public async Task<Hotel?> GetByIdWithIncludeAsync(int id)
    {
        return await _context.Hotels
            .Include(h => h.Place)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Hotel?> GetByIdAsync(int id)
    {
        string redisKey = "hotel" + id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            string jsonData = await _redis.StringGetAsync(redisKey);
            return JsonConvert.DeserializeObject<Hotel>(jsonData);
        }

        Hotel? hotel = await this.GetByIdWithIncludeAsync(id);

        if (hotel != null)
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(hotel), TimeSpan.FromMinutes(10));
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
        Hotel? hotel = await this.GetByIdWithIncludeAsync(hotelUpdate.Id);
        if (hotel == null) return false;

        bool isChangedForeignKey = hotel.PlaceId != hotelUpdate.PlaceId;

        hotel.Name = hotelUpdate.Name ?? hotel.Name;
        hotel.Address = hotelUpdate.Address ?? hotel.Address;
        hotel.Description = hotelUpdate.Description ?? hotel.Description;
        hotel.PricePerNight = hotelUpdate.PricePerNight;
        hotel.PlaceId = hotelUpdate.PlaceId != 0 ? hotelUpdate.PlaceId : hotel.PlaceId;
        hotel.ImageUrl = hotelUpdate.ImageUrl ?? hotel.ImageUrl;
        await _context.SaveChangesAsync();

        string redisKey = "hotel" + hotelUpdate.Id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            if (isChangedForeignKey) await _redis.StringSetAsync(redisKey, 
                JsonConvert.SerializeObject(await this.GetByIdWithIncludeAsync(hotel.Id)), TimeSpan.FromMinutes(10));
            else await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(hotel), TimeSpan.FromMinutes(10));
        }
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