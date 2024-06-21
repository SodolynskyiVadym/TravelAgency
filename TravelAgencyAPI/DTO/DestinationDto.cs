namespace TravelAgencyAPI.DTO;

public class DestinationDto : IDto
{
    public int Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TourId { get; set; }
    public int HotelId { get; set; }
    public int TransportId { get; set; }
}