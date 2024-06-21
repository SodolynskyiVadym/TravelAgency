namespace TravelAgencyAPI.DTO;

public class TransportDto : IDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public int QuantitySeats { get; set; }
    public string? ImageUrl { get; set; }
}