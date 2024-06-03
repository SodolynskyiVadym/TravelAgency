using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories;

public class PlaceRepository : IRepository<Place, PlaceDto>, IPlaceRepository
{
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;

    public PlaceRepository(TravelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Place?> GetByIdAsync(int id)
    {
        return await _context.Places
            .Include(p => p.ImagesUrls)
            .FirstOrDefaultAsync(place => place.Id == id);
    }

    public async Task<List<Place>> GetAllAsync()
    {
        return await _context.Places.ToListAsync();
    }
    
    public async Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo()
    {
        IEnumerable<Place> places = await this.GetAllAsync();
        return places.Select(place => _mapper.Map<PlaceInfoDto>(place));
    }
    
    
    public async Task<bool> AddAsync(PlaceDto place)
    {
        if (place.ImagesUrls == null || !place.ImagesUrls.Any()) return false;
        
        Place newPlace = new Place()
        {
            Name = place.Name!,
            Country = place.Country!,
            Description = place.Description!
        };
        await _context.Places.AddAsync(newPlace);
        await _context.SaveChangesAsync();
        
        var placeImageUrls = place.ImagesUrls.Select(url => new PlaceImageUrl { Url = url, PlaceId = newPlace.Id });
        await _context.PlaceImageUrls.AddRangeAsync(placeImageUrls);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, PlaceDto entity)
    {
        Place? place = await _context.Places.FindAsync(id);
        if (place == null) return false;
        
        place.Name = entity.Name ?? place.Name;
        place.Description = entity.Description ?? place.Description;
        place.Country = entity.Country ?? place.Country;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Place? place = await _context.Places.FindAsync(id);
        if (place == null) return false;

        _context.Places.Remove(place);
        await _context.SaveChangesAsync();

        return true;
    }
}