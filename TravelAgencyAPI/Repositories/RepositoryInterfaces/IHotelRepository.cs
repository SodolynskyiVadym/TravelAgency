using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IHotelRepository
{
    public Task<bool> AddHotelAsync(Hotel hotel);
}