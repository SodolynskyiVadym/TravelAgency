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
        return await _context.Places.Include(p => p.ImagesUrls).ToListAsync();
    }
    
    public async Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo()
    {
        IEnumerable<Place> places = await this.GetAllAsync();
        return places.Select(place => _mapper.Map<PlaceInfoDto>(place));
    }


    public async Task<int> AddAsync(PlaceDto placeDto)
    {
        if (placeDto.ImagesUrls == null || !placeDto.ImagesUrls.Any()) return 0;
        
        Place place = new Place()
        {
            Name = placeDto.Name!,
            Country = placeDto.Country!,
            Description = placeDto.Description!
        };
        await _context.Places.AddAsync(place);
        await _context.SaveChangesAsync();
        
        var placeImageUrls = placeDto.ImagesUrls.Select(url => new PlaceImageUrl { Url = url, PlaceId = place.Id });
        await _context.PlaceImageUrls.AddRangeAsync(placeImageUrls);
        await _context.SaveChangesAsync();
        return place.Id;
    }

    public async Task<bool> UpdateAsync(int id, PlaceDto placeUpdate)
    {
        Place? place = await _context.Places.Include(p => p.ImagesUrls).FirstOrDefaultAsync(p => p.Id == id);
        if (place == null) return false;
        
        _context.PlaceImageUrls.RemoveRange(place.ImagesUrls);
        
        place.Name = placeUpdate.Name ?? place.Name;
        place.Description = placeUpdate.Description ?? place.Description;
        place.Country = placeUpdate.Country ?? place.Country;

        place.ImagesUrls = placeUpdate.ImagesUrls?.Count() == 3 
            ? placeUpdate.ImagesUrls.Select(url => new PlaceImageUrl { Url = url, PlaceId = place.Id }).ToList() 
            : place.ImagesUrls;
        
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