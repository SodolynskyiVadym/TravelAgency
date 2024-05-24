namespace TravelAgencyAPI.DTO;

public class TransportDto : IDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int PricePerSeatPerKm { get; set; }
    public int QuantitySeats { get; set; }
}