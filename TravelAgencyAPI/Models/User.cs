using System.ComponentModel.DataAnnotations;

namespace TravelAgencyAPI.Models;
public class User : IModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }
    
    [Required]
    public byte[] PasswordSalt { get; set; }
    
    public byte[]? ReservePasswordHash { get; set; }
    public byte[]? ReservePasswordSalt { get; set; }

    [Required] 
    public string Role { get; set; } = "USER";
}