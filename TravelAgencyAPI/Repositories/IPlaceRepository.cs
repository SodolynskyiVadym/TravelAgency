using TravelAgencyAPI.DTO;

namespace TravelAgencyAPI.Repositories;

public interface IPlaceRepository
{
    public Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo();
}