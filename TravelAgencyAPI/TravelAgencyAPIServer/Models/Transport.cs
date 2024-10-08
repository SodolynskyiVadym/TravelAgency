﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPIServer.Models;

[Table("Transports")]
public class Transport : IModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Type { get; set; }
    
    [Required]
    public int QuantitySeats { get; set; }
    
    [Required]
    public string ImageUrl { get; set; }
    
    public Transport()
    {
    }
}