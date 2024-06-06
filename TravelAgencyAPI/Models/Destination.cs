using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Destinations")]
public class Destination : IModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
    
    public int TourId { get; set; }
    
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
    
    public int TransportId { get; set; }
    public Transport Transport { get; set; }
}