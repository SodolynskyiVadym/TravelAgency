using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Tours")]
public class Tour
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
    public int Price { get; set; }
    
    [Required]
    public int QuantitySeats { get; set; }
    
    public ICollection<Destination> Destinations { get; set; }
    
    
    public Tour()
    {
    }
}