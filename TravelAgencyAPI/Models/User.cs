﻿using System.ComponentModel.DataAnnotations;

namespace TravelAgencyAPI.Models;
public class User : IModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(30, ErrorMessage = "Name must be less than 30")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }
    
    [Required]
    public byte[] PasswordSalt { get; set; }

    [Required]
    public string Role { get; set; }
    
    public User()
    {
    }
}