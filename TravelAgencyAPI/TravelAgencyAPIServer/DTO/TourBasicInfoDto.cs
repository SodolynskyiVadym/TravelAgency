namespace TravelAgencyAPIServer.DTO;

public class TourBasicInfoDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ImageUrl { get; set; }
    public IEnumerable<string> DestinationsNames { get; set; }
}