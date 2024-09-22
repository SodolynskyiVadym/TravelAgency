namespace TravelAgencyService.Models;

public class TourEmailDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Price { get; set; }
    public int PurchasedSeats { get; set; }
    public string ImageUrl { get; set; }
    public string Email { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Description: {Description}, StartDate: {StartDate}, EndDate: {EndDate}, Price: {Price}, PurchasedSeats: {PurchasedSeats}";
    }
}