using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Destinations")]
public class Destination
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int TourId { get; set; }
    
    [ForeignKey("TourId")]
    public Tour Tour { get; set; }
    
    [Required]
    public int HotelId { get; set; }
    
    [ForeignKey("HotelId")]
    public Hotel Hotel { get; set; }
    
    [Required]
    public int TransportId { get; set; }
    
    [ForeignKey("TransportId")]
    public Transport Transport { get; set; }
    
    [Required]
    public int QuantityDays { get; set; }
    
    public Place Place => Hotel.Place;
    

    public Destination()
    {
    }
}