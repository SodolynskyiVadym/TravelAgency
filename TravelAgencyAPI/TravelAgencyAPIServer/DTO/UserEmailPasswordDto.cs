namespace TravelAgencyAPIServer.DTO;

public class UserEmailPasswordDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    public UserEmailPasswordDto(string email, string password)
    {
        Email = email;
        Password = password;
    }
    
    public UserEmailPasswordDto()
    {
    }
}