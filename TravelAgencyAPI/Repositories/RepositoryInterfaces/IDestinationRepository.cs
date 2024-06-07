using TravelAgencyAPI.DTO;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IDestinationRepository
{
    public Task<bool> UpdateDestinationsAsync(IEnumerable<DestinationDto> destinations, int tourId);
}