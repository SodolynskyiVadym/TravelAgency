using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;

namespace TravelAgencyAPI.Models;

[Table("Payments")]
public class Payment : IModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public bool IsPaid { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    public string? StripeSession { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; }
    
    [Required]
    public int TourId { get; set; }
    public Tour Tour { get; set; }
}