using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface ITourService
{
    public Task<List<Tour>> GetAvailableTours();
    public Task<List<Tour>> GetUnavailableTours();
    public Task CheckTourAvailability();
}