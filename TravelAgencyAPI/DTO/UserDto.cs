namespace TravelAgencyAPI.DTO;

public class UserDto : IDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "USER";
}