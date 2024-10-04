using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPIServer.Models;

public class Review : IModel
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public int Rating { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int TourId { get; set; }
    public Tour Tour { get; set; }
    
    
}