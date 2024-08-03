using TravelAgencyAPI.DTO;

namespace TravelAgencyAPI.Services.ServicesInterfaces;

public interface IPlaceService
{
    public Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo();
}