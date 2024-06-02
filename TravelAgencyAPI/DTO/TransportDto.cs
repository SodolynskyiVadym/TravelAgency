namespace TravelAgencyAPI.DTO;

public class TransportDto : IDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public double PriceForHundredKm { get; set; }
    public int QuantitySeats { get; set; }
    public string? ImageUrl { get; set; }
}