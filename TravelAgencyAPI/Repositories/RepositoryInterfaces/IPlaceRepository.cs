using TravelAgencyAPI.DTO;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IPlaceRepository
{
    public Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo();
}