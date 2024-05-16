using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Hotels")]
public class Hotel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public int PricePerNight { get; set; }
    
    [Required]
    public int PlaceId { get; set; }
    
    [ForeignKey("PlaceId")]
    public Place Place { get; set; }
    
    public Hotel()
    {
    }
}