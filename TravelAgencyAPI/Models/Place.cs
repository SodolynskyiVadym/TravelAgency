using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Places")]
public class Place
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    [Required]
    public string Description { get; set; }
    

    public string? AttractionSiteUrl { get; set; }
    
    [Required]
    public string ImageUrl { get; set; }
    
    
    public Place()
    {
    }
}