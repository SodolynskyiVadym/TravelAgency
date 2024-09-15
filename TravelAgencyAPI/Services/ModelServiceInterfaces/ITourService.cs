using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Services.ModelServiceInterfaces;

public interface ITourService
{
    public Task<List<TourBasicInfoDto>> GetAvailableTours();
    public Task<List<Tour>> GetUnavailableTours();
    public Task CheckTourAvailability();
}