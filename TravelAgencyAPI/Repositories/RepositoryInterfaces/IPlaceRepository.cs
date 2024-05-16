using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IPlaceRepository
{
    public Task<bool> AddPlaceAsync(Place place);
}