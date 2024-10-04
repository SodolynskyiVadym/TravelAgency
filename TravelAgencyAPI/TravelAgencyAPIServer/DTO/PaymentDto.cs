namespace TravelAgencyAPIServer.DTO;

public class PaymentDto : IDto
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public DateTime? Date { get; set; }
    public string? StripeSession { get; set; }
    public int TourId { get; set; }
    public int UserId { get; set; }
    public bool IsPaid { get; set; }
}