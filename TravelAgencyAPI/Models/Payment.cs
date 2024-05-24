using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAPI.Models;

[Table("Payments")]
public class Payment : IModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public bool Status { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    public User User { get; set; }
    public Tour Tour { get; set; }

    [NotMapped]
    public int TotalPrice => Amount * Tour.Price;
}