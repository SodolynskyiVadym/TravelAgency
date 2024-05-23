using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Payments")]
public class Payment : IModel
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    public Tour Tour { get; set; }
    
    [Required]
    public int QuantityPurchasedSeats { get; set; }

    [NotMapped]
    public int TotalPrice => QuantityPurchasedSeats * Tour.Price;

    public Payment()
    {
    }
}