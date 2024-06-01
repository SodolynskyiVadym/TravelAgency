﻿using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.DTO;

public class PlaceDto : IDto
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public IEnumerable<string>? ImagesUrls { get; set; }
}