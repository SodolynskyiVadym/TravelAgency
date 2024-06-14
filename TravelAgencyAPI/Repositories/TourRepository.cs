using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories;

public class TourRepository : IRepository<Tour, TourDto>
{
    private TravelDbContext _context;
    private readonly IMapper _mapper;

    public TourRepository(TravelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Tour?> GetByIdAsync(int id)
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
    
    public async Task<List<Tour>> GetAllAsync()
    {
        return await _context.Tours.ToListAsync();
    }
    
    public async Task<bool> AddAsync(TourDto tour)
    {
        await _context.Tours.AddAsync(_mapper.Map<Tour>(tour));
        await _context.SaveChangesAsync(); 
        return true;
    }
    
    
    public async Task<bool> UpdateAsync(int id, TourDto tourUpdate)
    {
        Tour? tour = await _context.Tours.Include(t => t.Destinations).FirstOrDefaultAsync(t => t.Id == id);
        if (tour == null) return false;

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
        tour.IsAvailable = tourUpdate.IsAvailable;
        tour.Destinations = tourUpdate.Destinations?.Count() > 0 ? tourUpdate.Destinations.Select(d => _mapper.Map<Destination>(d)).ToList() : tour.Destinations;
        
        await _context.SaveChangesAsync();
        return true;
    }  
    
    public async Task<bool> DeleteAsync(int id)
    {
        Tour? tour = await _context.Tours.FindAsync(id);
        if (tour == null) return false;
        
        _context.Tours.Remove(tour);
        await _context.SaveChangesAsync();
        
        return true;
    }
}