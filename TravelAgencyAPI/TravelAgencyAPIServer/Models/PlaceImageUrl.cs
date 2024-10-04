using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPIServer.Models;

public class PlaceImageUrl
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int PlaceId { get; set; }
}