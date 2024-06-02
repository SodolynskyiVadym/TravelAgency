namespace TravelAgencyAPI.DTO;

public class TourDto : IDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public int QuantitySeats { get; set; }
    public string? ImageUrl { get; set; }
}