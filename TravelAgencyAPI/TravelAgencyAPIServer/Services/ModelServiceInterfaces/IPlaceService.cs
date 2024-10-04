using TravelAgencyAPIServer.DTO;

namespace TravelAgencyAPIServer.Services.ModelServiceInterfaces;

public interface IPlaceService
{
    public Task<IEnumerable<PlaceInfoDto>> GetPlacesInfo();
}