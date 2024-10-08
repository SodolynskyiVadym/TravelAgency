﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPIServer.Models;

[Table("Places")]
public class Place : IModel
{
    [Key]
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