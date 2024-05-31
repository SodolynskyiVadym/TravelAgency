using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Places")]
public class Place : IModel
{
    [Key]
    [Index]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public IEnumerable<PlaceImageUrl> ImagesUrls { get; set; }
    
    public Place()
    {
    }
}