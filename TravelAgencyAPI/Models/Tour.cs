using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Tours")]
public class Tour
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
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