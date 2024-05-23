﻿namespace TravelAgencyAPI.DTO;

public class PlaceCreateDto : IDto
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public string? SiteUrl { get; set; }
}