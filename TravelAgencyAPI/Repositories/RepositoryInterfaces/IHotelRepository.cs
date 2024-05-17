using TravelAgencyAPI.DTO;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IHotelRepository
{
    public Task<bool> AddHotelAsync(HotelCreateDto hotel);
}