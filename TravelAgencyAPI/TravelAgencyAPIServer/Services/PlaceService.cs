﻿using System.Collections;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services.Interfaces;
using TravelAgencyAPIServer.Services.ModelServiceInterfaces;

namespace TravelAgencyAPIServer.Services;

public class PlaceService : IRepository<Place, PlaceDto>, IPlaceService
{
    private readonly TravelDbContext _context;
    private readonly DapperDbContext _dapperDbContext;
    private readonly IMapper _mapper;
    private readonly IDatabase _redis;
    private readonly ComparatorHelper _comparatorHelper = new();

    public PlaceService(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redisConnecction, DapperDbContext dapperDbContext)
    {
        _context = context;
        _mapper = mapper;
        _dapperDbContext = dapperDbContext;
        _redis = redisConnecction.GetDatabase();
    }

    public async Task<Place?> GetByIdWithIncludeAsync(int id)
    {
        Console.WriteLine("GetByIdWithIncludeAsync");
        string query = $@"SELECT p.*, pi.url
        FROM Places p
        LEFT JOIN PlaceImageUrls pi ON p.Id = pi.PlaceId
        WHERE p.Id = {id}";
        return await _dapperDbContext.Connection.QueryFirstOrDefaultAsync<Place>(query);
        // return await _context.Places
        //     .Include(p => p.ImagesUrls)
        //     .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Place?> GetByIdAsync(int id)
    {
        string redisKey = "place" + id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            string jsonData = await _redis.StringGetAsync(redisKey);
            return JsonConvert.DeserializeObject<Place>(jsonData);
        }

        Place? place = await this.GetByIdWithIncludeAsync(id);
        
        if(place != null) 
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(place), TimeSpan.FromMinutes(10));
        return place;
    }
    
    public async Task<IEnumerable<int>> GetUsedPlacesIds()
    {
        string query = "SELECT PlaceId FROM Hotels UNION SELECT PlaceStartId FROM Tours UNION SELECT PlaceEndId FROM Tours;";
        return await _dapperDbContext.Connection.QueryAsync<int>(query);
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

    public async Task<bool> UpdateAsync(PlaceDto placeUpdate)
    {
        Place? place = await this.GetByIdWithIncludeAsync(placeUpdate.Id);
        if (place == null) return false;
        
        bool isChangedForeignKey = _comparatorHelper.IsPlaceImageUrlsChanged(place.ImagesUrls, placeUpdate.ImagesUrls);
        
        _context.PlaceImageUrls.RemoveRange(place.ImagesUrls);
        
        place.Name = placeUpdate.Name ?? place.Name;
        place.Description = placeUpdate.Description ?? place.Description;
        place.Country = placeUpdate.Country ?? place.Country;

        place.ImagesUrls = placeUpdate.ImagesUrls?.Count() == 3 
            ? placeUpdate.ImagesUrls.Select(url => new PlaceImageUrl { Url = url, PlaceId = place.Id }).ToList() 
            : place.ImagesUrls;
        
        await _context.SaveChangesAsync();
        
        string redisKey = "place" + placeUpdate.Id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            if(isChangedForeignKey) await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(await this.GetByIdWithIncludeAsync(place.Id)), TimeSpan.FromMinutes(10));
            else await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(place), TimeSpan.FromMinutes(10));
        }

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Place? place = await _context.Places.FindAsync(id);
        if (place == null) return false;

        _context.Places.Remove(place);
        await _context.SaveChangesAsync();
        await _redis.KeyDeleteAsync("place" + id);
        return true;
    }
}