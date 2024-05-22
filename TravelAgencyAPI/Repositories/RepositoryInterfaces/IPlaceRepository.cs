using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IPlaceRepository
{
    public Task<Place?> GetAsyncById(int id);
    public Task<List<Place>> GetAllPlacesListAsync();
    public Task<bool> AddPlaceAsync(PlaceCreateDto place);
}