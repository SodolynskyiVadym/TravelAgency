using System.ComponentModel.DataAnnotations;
using TravelAgencyAPI.Models;

public class User
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
    
    public ICollection<Payment> Payments { get; set; }
    
    public User()
    {
    }
}