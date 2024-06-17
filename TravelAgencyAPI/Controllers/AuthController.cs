using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;
using TravelAgencyAPI.Settings;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly AuthHelper _authHelper;
    private readonly IMapper _mapper;
    
    public AuthController(TravelDbContext context, IMapper mapper, IOptions<AuthSetting> authSetting, IOptions<MailSetting> mailSetting)
    {
        _userRepository = new UserRepository(context, mapper);
        _authHelper = new AuthHelper(authSetting, mailSetting, _userRepository);
        _mapper = mapper;
    }
    
    [Authorize(Roles = "ADMIN")]
    [HttpGet("{id}")]
    public async Task<UserEmailRoleDto?> GetUserById(int id)
    {
        return _mapper.Map<UserEmailRoleDto>(await _userRepository.GetByIdAsync(id));
    }
    
    
    [Authorize]
    [HttpGet("getUserByToken")]
    public async Task<UserEmailRoleDto?> GetUserByToken(int id)
    {
        int userId = 0;
        int.TryParse(User.FindFirst("userId")?.Value, out userId);
        return _mapper.Map<UserEmailRoleDto>(await _userRepository.GetByIdAsync(userId));
    }
    
    
    [Authorize(Roles = "ADMIN")]
    [HttpGet("getAllUsers")]
    public async Task<List<UserEmailRoleDto>> GetAllUsers()
    {
        return (await _userRepository.GetAllAsync()).Select(u => _mapper.Map<UserEmailRoleDto>(u)).ToList();
    }
    
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRegistrationDto userLogin)
    {
        User? user = await _userRepository.GetUserByEmail(userLogin.Email);
        if (user == null) return StatusCode(400, "User not found!");
        
        var passwordHash = _authHelper.GetPasswordHash(userLogin.Password, user.PasswordSalt);

        for (var index = 0; index < passwordHash.Length; index++)
        {
            if (passwordHash[index] != user.PasswordHash[index]) return StatusCode(400, "Incorrect password!");
        }
            
        
        return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user)}}); 
    }
    
    
    [HttpPost("registerUser")]
    public async Task<IActionResult> Register(UserLoginRegistrationDto userRegistration)
    {
        userRegistration.Role = "USER";
        if (userRegistration.Email.IsNullOrEmpty() || userRegistration.Password.Length < 8)
        {
            return StatusCode(400, "Email is empty or password is less than 8");
        }

        IEnumerable<User> users = await _userRepository.GetAllAsync();
        if (users.FirstOrDefault(u => u.Email == userRegistration.Email) != null) return StatusCode(400, "User already exists");

        bool isRegistered = await _authHelper.RegisterUser(userRegistration, "USER");
        if (isRegistered)
        {
            User user = await _userRepository.GetUserByEmail(userRegistration.Email) ?? throw new InvalidOperationException();
            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user)}});
        }

        return StatusCode(400, "Incorrect data entered");
    }
    
    
    [Authorize(Roles = "ADMIN")]
    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser(UserEmailRoleDto user)
    {
        if (user.Role.IsNullOrEmpty() || (user.Role != "EDITOR" && user.Role != "ADMIN"))
            return BadRequest("Incorrect data");

        await _authHelper.CreateUser(user);
        return Ok(user);
    }

    
    [Authorize]
    [HttpPost("updatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] string password)
    {
        string? id = User.FindFirst("userId")?.Value;
        if (id == null) return StatusCode(402, "Incorrect token!");

        Console.WriteLine(id);
        
        User? user = await _userRepository.GetByIdAsync(int.Parse(id)); 
        if(user == null) return StatusCode(402, "User not found!");

        Console.WriteLine(password);
        
        if (await _authHelper.UpdatePassword(user, password)) return Ok();
        return BadRequest("Password is less than 7 characters");
    }
    
    
    [Authorize(Roles = "ADMIN")]
    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (await _userRepository.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}