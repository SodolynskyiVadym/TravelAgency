using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IPlaceRepository
{
    public Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo();
}