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
    
    [NotMapped]
    public int TourId { get; set; }
    public Tour Tour { get; set; }
    
    [NotMapped]
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
    
    [NotMapped]
    public int TransportId { get; set; }
    public Transport Transport { get; set; }
}