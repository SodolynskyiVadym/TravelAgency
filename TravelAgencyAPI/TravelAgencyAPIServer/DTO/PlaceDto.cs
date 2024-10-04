using TravelAgencyAPIServer.Models;

namespace TravelAgencyAPIServer.DTO;

public class PlaceDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public IEnumerable<string>? ImagesUrls { get; set; }
}