namespace TravelAgencyAPI.DTO;

public class UserLoginRegistrationDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Role { get; set; }
}