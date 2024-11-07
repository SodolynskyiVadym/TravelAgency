using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Models;

namespace TravelAgencyAPIServer.Services.ModelServiceInterfaces;

public interface ITourService
{
    public Task<IEnumerable<TourBasicInfoDto>> GetAvailableTours();
    public Task<List<Tour>> GetUnavailableTours();
    public Task CheckTourAvailability();
}