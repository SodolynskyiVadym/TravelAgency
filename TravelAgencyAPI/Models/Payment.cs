using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Payments")]
public class Payment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
    
    [Required]
    public int TourId { get; set; }

    [ForeignKey("TourId")]
    public Tour Tour { get; set; }
    
    [Required]
    public int QuantityPurchasedSeats { get; set; }

    [NotMapped]
    public int TotalPrice => QuantityPurchasedSeats * Tour.Price;

    public Payment()
    {
    }
}