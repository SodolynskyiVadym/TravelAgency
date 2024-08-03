using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;
using TravelAgencyAPI.Services;
using TravelAgencyAPI.Settings;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthHelper _authHelper;
    private readonly MailHelper _mailHelper;
    private readonly IMapper _mapper;
    
    public AuthController(TravelDbContext context, IMapper mapper, IOptions<AuthSetting> authSetting, IOptions<MailSetting> mailSetting)
    {
        _userService = new UserService(context, mapper);
        _authHelper = new AuthHelper(authSetting, _userService);
        _mailHelper = new MailHelper(mailSetting.Value);
        _mapper = mapper;
    }
    
    [Authorize(Roles = "ADMIN")]
    [HttpGet("{id}")]
    public async Task<UserEmailRoleDto?> GetUserById(int id)
    {
        return _mapper.Map<UserEmailRoleDto>(await _userService.GetByIdAsync(id));
    }
    
    
    [Authorize]
    [HttpGet("getUserByToken")]
    public async Task<UserEmailRoleDto?> GetUserByToken(int id)
    {
        int userId = 0;
        int.TryParse(User.FindFirst("userId")?.Value, out userId);
        return _mapper.Map<UserEmailRoleDto>(await _userService.GetByIdAsync(userId));
    }
    
    
    [Authorize(Roles = "ADMIN")]
    [HttpGet("getAllUsers")]
    public async Task<List<UserEmailRoleDto>> GetAllUsers()
    {
        return (await _userService.GetAllAsync()).Select(u => _mapper.Map<UserEmailRoleDto>(u)).ToList();
    }
    
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRegistrationDto userLogin)
    {
        User? user = await _userService.GetUserByEmail(userLogin.Email);
        if (user == null) return StatusCode(400, "User not found!");
        
        if(!_authHelper.CheckPassword(userLogin.Password, user.PasswordHash, user.PasswordSalt))
            return StatusCode(400, "Incorrect password!");
        return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user)}}); 
    }
    
    
    [HttpPost("loginViaReservePassword")]
    public async Task<IActionResult> LoginViaReservePassword(UserLoginRegistrationDto userLogin)
    {
        User? user = await _userService.GetUserByEmail(userLogin.Email);
        if (user == null) return StatusCode(400, "User not found!");
        
        if (!_authHelper.CheckPassword(userLogin.Password, user.ReservePasswordHash, user.ReservePasswordSalt))
            return StatusCode(400, "Incorrect password!");

        await _userService.RemoveReservePassword(userLogin.Email);
        return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user)}}); 
    }


    [HttpPost("createReservePassword")]
    public async Task<IActionResult> CreateReservePassword(string email)
    {
        if(email.IsNullOrEmpty()) return StatusCode(401, "Email is empty");

        string password = await _authHelper.CreateReservePassword(email);
        if (password.IsNullOrEmpty()) return BadRequest("User not found!");
        
        if(_mailHelper.SendReservePassword(email, password)) return Ok();
        return StatusCode(500, "Error sending email");
    }
    
    
    
    [HttpPost("registerUser")]
    public async Task<IActionResult> Register(UserLoginRegistrationDto userRegistration)
    {
        userRegistration.Role = "USER";
        if (userRegistration.Email.IsNullOrEmpty() || userRegistration.Password.Length < 8)
        {
            return StatusCode(400, "Email is empty or password is less than 8");
        }

        if(await _userService.IsUsedEmail(userRegistration.Email)) return BadRequest("Email is already used!");

        bool isRegistered = await _authHelper.RegisterUser(userRegistration) > 0;
        if (isRegistered)
        {
            User user = await _userService.GetUserByEmail(userRegistration.Email) ?? throw new InvalidOperationException();
            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user)}});
        }

        return StatusCode(400, "IUser was not registered!");
    }
    
    
    [Authorize(Roles = "ADMIN")]
    [HttpPost("registerEditorAdmin")]
    public async Task<IActionResult> CreateUser(UserEmailRoleDto user)
    {
        if (user.Role.IsNullOrEmpty() || (user.Role != "EDITOR" && user.Role != "ADMIN"))
            return BadRequest("Incorrect data");

        string password = await _authHelper.RegisterEditorAdmin(user);
        if(password.IsNullOrEmpty()) return BadRequest("User already exists");
        
        if(_mailHelper.SendPassword(user.Email, password, user.Role)) return Ok();
        return StatusCode(500, "Error sending email");
    }

    
    [Authorize]
    [HttpPost("updatePassword")]
    public async Task<IActionResult> UpdatePassword(UserUpdatePasswordDto userPassword)
    {
        string? id = User.FindFirst("userId")?.Value;
        if (id == null) return StatusCode(402, "Incorrect token!");
        
        User? user = await _userService.GetByIdAsync(int.Parse(id)); 
        if(user == null) return StatusCode(402, "User not found!");
        
        if (await _authHelper.UpdatePassword(user, userPassword.Password)) return Ok();
        return BadRequest("Password is less than 7 characters");
    }
    
    
    [Authorize(Roles = "ADMIN")]
    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (await _userService.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}