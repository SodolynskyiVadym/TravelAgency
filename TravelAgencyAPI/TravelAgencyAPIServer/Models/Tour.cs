using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPIServer.Models;

[Table("Tours")]
public class Tour : IModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MinLength(5, ErrorMessage = "Name of tour must be more than 4 symbols")]
    public string Name { get; set; }
    
    [Required]
    [MinLength(20, ErrorMessage = "Description must be more than 19 symbols")]
    public string Description { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public int Price { get; set; }
    
    [Required]
    public int QuantitySeats { get; set; }
    
    [Required]
    public string ImageUrl { get; set; }
    
    [Required]
    public bool IsAvailable { get; set; }
    

    [Required]
    public int PlaceStartId { get; set; }

    public Place PlaceStart { get; set; }
    
    [Required]
    public int PlaceEndId { get; set; }
    public Place PlaceEnd { get; set; }
    
    [Required]
    public int TransportToEndId { get; set; }
    public Transport TransportToEnd { get; set; }
    
    public IEnumerable<Destination> Destinations { get; set; }
    
    
    public Tour()
    {
    }
}