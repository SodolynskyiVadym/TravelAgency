using TravelAgencyAPI.DTO;

namespace TravelAgencyAPI.Services.ModelServiceInterfaces;

public interface IPlaceService
{
    public Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo();
}