using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IPlaceService
{
    public Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo();
}