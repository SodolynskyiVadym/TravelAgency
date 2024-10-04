using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Models;

namespace TravelAgencyAPIServer.Helpers;

public class ComparatorHelper
{
    private bool IsDestinationChanged(Destination destination, DestinationDto updatedDestination)
    {
        return destination.Id != updatedDestination.Id || destination.StartDate != updatedDestination.StartDate
               || destination.EndDate != updatedDestination.EndDate || destination.HotelId != updatedDestination.HotelId
               || destination.TransportId != updatedDestination.TransportId;
    }
    
    
    public bool IsTourDestinationsChanged(IEnumerable<Destination> destinations, IEnumerable<DestinationDto> updatedDestinations)
    {
        if (!updatedDestinations.Any()) return false;
        if (destinations.Count() != updatedDestinations.Count()) return true;
        destinations = destinations.OrderBy(d => d.StartDate).ToList();
        updatedDestinations = updatedDestinations.OrderBy(d => d.StartDate).ToList();

        for (int i = 0; i < destinations.Count(); i++)
        {
            var destination = destinations.ElementAt(i);
            var updatedDestination = updatedDestinations.ElementAt(i);
            if (this.IsDestinationChanged(destination, updatedDestination)) return true;
        }
        return false;
    }
    
    public bool IsPlaceImageUrlsChanged(IEnumerable<PlaceImageUrl> placeImageUrls, IEnumerable<string>? updatedImageUrls)
    {
        if (updatedImageUrls.Count() != 3) return false;
        
        List<string> placeImageUrlsList = placeImageUrls.Select(p => p.Url).ToList();
        foreach (var updatedImageUrl in updatedImageUrls)
        {
            if (!placeImageUrlsList.Contains(updatedImageUrl)) return true;
        }

        return false;
    }
}