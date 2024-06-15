﻿namespace TravelAgencyAPI.DTO;

public class UserDto : IDto
{ 
    public string? Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; } 
    public string? Role { get; set; }
}