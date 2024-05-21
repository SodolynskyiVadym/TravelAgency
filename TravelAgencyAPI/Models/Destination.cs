using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Destinations")]
public class Destination
{
    [Key]
    public int Id { get; set; }
    public Tour Tour { get; set; }
    public Hotel Hotel { get; set; }
    public Transport Transport { get; set; }
    public Place Place { get; set; }
    
    // Remake with DateTime
    // [Required]
    // public int QuantityDays { get; set; }
    
    public Destination()
    {
    }
}