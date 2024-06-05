namespace TravelAgencyAPI.DTO;

public class TourDto : IDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int QuantitySeats { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int PlaceStartId { get; set; }
    public int PlaceEndId { get; set; }
    public int TransportToEndId { get; set; }
}