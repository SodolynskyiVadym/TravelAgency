﻿using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IHotelRepository
{
    public Task<Hotel?> GetHotelByIdAsync(int id);
    public Task<List<Hotel>> GetAllHotelsListAsync();
    public Task<bool> AddHotelAsync(HotelCreateDto hotel);
    public Task<bool> UpdateHotelAsync(int id, HotelCreateDto hotel);
    public Task<bool> DeleteHotelAsync(int id);
}