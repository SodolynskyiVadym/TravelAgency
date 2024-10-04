namespace TravelAgencyAPIServer.DTO;

public class ReviewDto : IDto
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public int Rating { get; set; } = 3;
    public int UserId { get; set; }
    public int TourId { get; set; }
}