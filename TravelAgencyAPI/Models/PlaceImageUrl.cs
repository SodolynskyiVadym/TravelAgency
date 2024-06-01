using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

public class PlaceImageUrl
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int PlaceId { get; set; }
    
    [NotMapped]
    public Place Place { get; set; }
}