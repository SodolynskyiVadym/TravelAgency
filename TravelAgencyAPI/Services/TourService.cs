using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Services.RepositorieInterfaces;
using TravelAgencyAPI.Services.ServicesInterfaces;

namespace TravelAgencyAPI.Services;

public class TourService : IRepository<Tour, TourDto>, ITourService
{
    private TravelDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDatabase _redis;
    private readonly ComparatorHelper _comparatorHelper = new();

    public TourService(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redisConnection)
    {
        _context = context;
        _mapper = mapper;
        _redis = redisConnection.GetDatabase();
    }

    public async Task<Tour?> GetByIdWithIncludeAsync(int id)
    {
        return await _context.Tours
            .Include(t => t.PlaceStart)
            .ThenInclude(p => p.ImagesUrls)
            .Include(t => t.PlaceEnd)
            .ThenInclude(p => p.ImagesUrls)
            .Include(t => t.Destinations)
            .ThenInclude(d => d.Hotel)
            .ThenInclude(h => h.Place)
            .ThenInclude(p => p.ImagesUrls)
            .Include(t => t.Destinations)
            .ThenInclude(d => d.Transport)
            .Include(t => t.TransportToEnd)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tour?> GetByIdAsync(int id)
    {
        string redisKey = "tour" + id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            string jsonData = await _redis.StringGetAsync(redisKey);
            return JsonConvert.DeserializeObject<Tour>(jsonData);
        }

        Tour? tour = await this.GetByIdWithIncludeAsync(id);
        if (tour != null)
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(tour), TimeSpan.FromMinutes(10));
        return tour;
    }

    public async Task<List<Tour>> GetAllAsync()
    {
        return await _context.Tours.ToListAsync();
    }

    public async Task<int> AddAsync(TourDto tourDto)
    {
        Tour tour = _mapper.Map<Tour>(tourDto);
        await _context.Tours.AddAsync(tour);
        await _context.SaveChangesAsync();
        return tour.Id;
    }


    public async Task<bool> UpdateAsync(TourDto tourUpdate)
    {
        Tour? tour = await this.GetByIdWithIncludeAsync(tourUpdate.Id);
        if (tour == null) return false;

        tour.Destinations = tour.Destinations.OrderBy(d => d.StartDate).ToList();
        tourUpdate.Destinations = tourUpdate.Destinations.OrderBy(d => d.StartDate).ToList();

        bool isChangedForeignKey = tour.PlaceStartId != tourUpdate.PlaceStartId || tour.PlaceEndId != tourUpdate.PlaceEndId 
            || tour.TransportToEndId != tourUpdate.TransportToEndId || _comparatorHelper.IsTourDestinationsChanged(tour.Destinations, tourUpdate.Destinations);

        _context.Destinations.RemoveRange(tour.Destinations);
        tour.Name = tourUpdate.Name ?? tour.Name;
        tour.Description = tourUpdate.Description ?? tour.Description;
        tour.Price = tourUpdate.Price;
        tour.QuantitySeats = tourUpdate.QuantitySeats;
        tour.ImageUrl = tourUpdate.ImageUrl ?? tour.ImageUrl;
        tour.PlaceStartId = tourUpdate.PlaceStartId != 0 ? tourUpdate.PlaceStartId : tour.PlaceStartId;
        tour.PlaceEndId = tourUpdate.PlaceEndId != 0 ? tourUpdate.PlaceEndId : tour.PlaceEndId;
        tour.TransportToEndId = tourUpdate.TransportToEndId != 0 ? tourUpdate.TransportToEndId : tour.TransportToEndId;
        tour.StartDate = tourUpdate.StartDate ?? tour.StartDate;
        tour.EndDate = tourUpdate.EndDate ?? tour.EndDate;
        tour.IsAvailable = tour.StartDate > DateTime.Now;
        tour.Destinations = tourUpdate.Destinations?.Count() > 0
            ? tourUpdate.Destinations.Select(d => _mapper.Map<Destination>(d)).ToList()
            : tour.Destinations;

        await _context.SaveChangesAsync();

        string redisKey = "tour" + tourUpdate.Id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            if (isChangedForeignKey) await _redis.StringSetAsync(redisKey,
                    JsonConvert.SerializeObject(await this.GetByIdWithIncludeAsync(tour.Id)), TimeSpan.FromMinutes(10));
            else await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(tour), TimeSpan.FromMinutes(10));
        }

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Tour? tour = await _context.Tours.FindAsync(id);
        if (tour == null) return false;

        _context.Tours.Remove(tour);
        await _context.SaveChangesAsync();
        await _redis.KeyDeleteAsync("tour" + id);
        return true;
    }

    public async Task<List<TourBasicInfoDto>> GetAvailableTours()
    {
        return await _context.Tours.Include(t => t.Destinations)
            .ThenInclude(d => d.Hotel)
            .ThenInclude(h => h.Place)
            .Where(t => t.IsAvailable)
            .Select(t => _mapper.Map<TourBasicInfoDto>(t))
            .ToListAsync();
    }

    public async Task<List<Tour>> GetUnavailableTours()
    {
        return await _context.Tours.Where(t => !t.IsAvailable).ToListAsync();
    }

    public async Task CheckTourAvailability()
    {
        List<Tour> tours = await _context.Tours.Where(t => t.StartDate < DateTime.Now).ToListAsync();
        foreach (var tour in tours)
        {
            tour.IsAvailable = false;
        }

        await _context.SaveChangesAsync();
    }
}