﻿namespace TravelAgencyAPI.DTO;

public class HotelDto : IDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public int PricePerNight { get; set; }
    public string? ImageUrl { get; set; }
    public int PlaceId { get; set; }
}