﻿namespace TravelAgencyAPI.DTO;

public class PaymentDbo
{
    public int Amount { get; set; }
    public DateTime? Date { get; set; }
    public int TourId { get; set; }
    public int UserId { get; set; }
    public bool PaymentStatus { get; set; }
}