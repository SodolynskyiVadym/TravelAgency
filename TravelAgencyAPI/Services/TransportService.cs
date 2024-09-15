using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Services.Interfaces;

namespace TravelAgencyAPI.Services;

public class TransportService : IRepository<Transport, TransportDto>
{
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDatabase _redis;
    private IRepository<Transport, TransportDto> _repositoryImplementation;

    public TransportService(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redisConnection)
    {
        _context = context;
        _mapper = mapper;
        _redis = redisConnection.GetDatabase();
    }

    public async Task<Transport?> GetByIdWithIncludeAsync(int id)
    {
        return await _context.Transports.FindAsync(id);
    }

    public async Task<Transport?> GetByIdAsync(int id)
    {
        string redisKey = "transport" + id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            string jsonData = await _redis.StringGetAsync(redisKey);
            return JsonConvert.DeserializeObject<Transport>(jsonData);
        }
        Transport? transport = await _context.Transports.FindAsync(id);
        if(transport != null) 
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(transport), TimeSpan.FromMinutes(10));
        return transport;
    }
    
    public async Task<List<Transport>> GetAllAsync()
    {
        return await _context.Transports.ToListAsync();
    }
    
    public async Task<int> AddAsync(TransportDto transportDto)
    {
        Transport transport = _mapper.Map<Transport>(transportDto);
        await _context.Transports.AddAsync(transport);
        await _context.SaveChangesAsync();
        return transport.Id;
    }
    
    public async Task<bool> UpdateAsync(TransportDto transportUpdate)
    {
        Transport? transport = await _context.Transports.FindAsync(transportUpdate.Id);
        if (transport == null) return false;
        
        transport.Name = transportUpdate.Name ?? transport.Name;
        transport.Description = transportUpdate.Description ?? transport.Description;
        transport.Type = transportUpdate.Type ?? transport.Type;
        transport.QuantitySeats = transportUpdate.QuantitySeats;
        transport.ImageUrl = transportUpdate.ImageUrl ?? transport.ImageUrl;
        await _context.SaveChangesAsync();
        
        string redisKey = "transport" + transportUpdate.Id;
        if (await _redis.KeyExistsAsync(redisKey))
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(transport), TimeSpan.FromMinutes(10));
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Transport? transport = await _context.Transports.FindAsync(id);
        if(transport == null) return false;
        
        _context.Transports.Remove(transport);
        await _context.SaveChangesAsync();
        await _redis.KeyDeleteAsync("transport" + id);
        return true;
    }
}