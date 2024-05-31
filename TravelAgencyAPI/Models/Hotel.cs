using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;

namespace TravelAgencyAPI.Models;

[Table("Hotels")]
public class Hotel : IModel
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
    public string ImageUrl { get; set; }
    
    public int PlaceId { get; set; }
    public Place Place { get; set; }
    
    public Hotel()
    {
    }
}