﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services;
using TravelAgencyAPIServer.Services.Interfaces;
using TravelAgencyAPIServer.Settings;

namespace TravelAgencyAPIServer.Controllers;

/// <summary>
/// The AuthController class handles authentication and authorization operations for the Travel Agency API.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthHelper _authHelper;
    private readonly IMapper _mapper;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    /// <summary>
    /// Initializes a new instance of the AuthController class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mapper">The object mapper.</param>
    /// <param name="authSetting">The authentication settings.</param>
    /// <param name="rabbitMqPublisher">The RabbitMQ publisher.</param>
    public AuthController(TravelDbContext context, IMapper mapper, IOptions<AuthSetting> authSetting,
        IRabbitMqPublisher rabbitMqPublisher)
    {
        _userService = new UserService(context, mapper);
        _authHelper = new AuthHelper(authSetting.Value);
        _mapper = mapper;
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user details.</returns>
    [Authorize(Roles = "ADMIN")]
    [HttpGet("{id}")]
    public async Task<UserEmailRoleDto?> GetUserById(int id)
    {
        return _mapper.Map<UserEmailRoleDto>(await _userService.GetByIdAsync(id));
    }

    /// <summary>
    /// Retrieves the authenticated user by their token.
    /// </summary>
    /// <returns>The user details.</returns>
    [Authorize]
    [HttpGet("getUserByToken")]
    public async Task<UserEmailRoleDto?> GetUserByToken()
    {
        int.TryParse(User.FindFirst("userId")?.Value, out var userId);
        return _mapper.Map<UserEmailRoleDto>(await _userService.GetByIdAsync(userId));
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of all users.</returns>
    [Authorize(Roles = "ADMIN")]
    [HttpGet("getAllUsers")]
    public async Task<List<UserIdEmailRoleDto>> GetAllUsers()
    {
        return (await _userService.GetAllAsync()).Select(u => _mapper.Map<UserIdEmailRoleDto>(u)).ToList();
    }

    /// <summary>
    /// Authenticates a user and returns a token.
    /// </summary>
    /// <param name="userLogin">The user login details.</param>
    /// <returns>The authentication token.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserEmailPasswordDto userLogin)
    {
        User? user = await _userService.GetUserByEmail(userLogin.Email);
        if (user == null) return StatusCode(400, "User not found!");
        
        if (!_authHelper.CheckPassword(userLogin.Password, user.PasswordHash, user.PasswordSalt))
            return StatusCode(400, "Incorrect password!");

        return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user) } });
    }

    /// <summary>
    /// Authenticates a user using a reserve password and returns a token.
    /// </summary>
    /// <param name="userLogin">The user login details.</param>
    /// <returns>The authentication token.</returns>
    [HttpPost("loginViaReservePassword")]
    public async Task<IActionResult> LoginViaReservePassword(UserEmailPasswordDto userLogin)
    {
        User? user = await _userService.GetUserByEmail(userLogin.Email);
        if (user == null) return StatusCode(400, "User not found!");

        if (!_authHelper.CheckPassword(userLogin.Password, user.ReservePasswordHash, user.ReservePasswordSalt))
            return StatusCode(400, "Incorrect password!");

        await _userService.RemoveReservePassword(userLogin.Email);
        return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user) } });
    }

    /// <summary>
    /// Creates a reserve password for a user and sends it via email.
    /// </summary>
    /// <param name="userParam">The user's email address.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [HttpPost("createReservePassword")]
    public async Task<IActionResult> CreateReservePassword([FromBody] UserParamDto userParam)
    {
        string email = userParam.Email;
        if(email.IsNullOrEmpty()) return StatusCode(401, "Email is empty");

        string password = _authHelper.GenerateRandomPassword();
        byte[][] passwordDetails = _authHelper.EncryptUserPassword(password);
        bool result = await _userService.CreateReservePasswordAsync(email, passwordDetails[0], passwordDetails[1]);

        if (!result) return BadRequest("User not found!");

        UserEmailPasswordDto user = new UserEmailPasswordDto(email, password);
        await _rabbitMqPublisher.PublishAsync(user, "reserve-password-queue");
        return Ok();
    }

    /// <summary>
    /// Registers a new user and returns a token.
    /// </summary>
    /// <param name="userRegistration">The user registration details.</param>
    /// <returns>The authentication token.</returns>
    [HttpPost("signup")]
    public async Task<IActionResult> Register(UserEmailPasswordDto userRegistration)
    {
        if (userRegistration.Password.Length < 8 || !AuthHelper.IsEmail(userRegistration.Email))
        {
            return StatusCode(400, "Email is incorrect or password is less than 8");
        }

        if (await _userService.IsUsedEmail(userRegistration.Email)) return BadRequest("Email is already used!");

        byte[][] passwordDetails = _authHelper.EncryptUserPassword(userRegistration.Password);
        UserDto userDto = new UserDto()
        {
            Email = userRegistration.Email,
            PasswordHash = passwordDetails[0],
            PasswordSalt = passwordDetails[1],
            Role = "USER"
        };
        await _userService.AddAsync(userDto);
        
        User user = await _userService.GetUserByEmail(userRegistration.Email) ?? throw new InvalidOperationException();
        return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(user) } });
    }

    /// <summary>
    /// Registers a new editor or admin user and sends their details via email.
    /// </summary>
    /// <param name="user">The user details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "ADMIN")]
    [HttpPost("createEditorAdmin")]
    public async Task<IActionResult> CreateUser(UserEmailRoleDto user)
    {
        if ((user.Role != "EDITOR" && user.Role != "ADMIN") || !AuthHelper.IsEmail(user.Email))
            return BadRequest("Incorrect data");
        
        string password = _authHelper.GenerateRandomPassword();
        byte[][] passwordDetails = _authHelper.EncryptUserPassword(password);
        UserDto userDto = new UserDto()
        {
            Email = user.Email,
            PasswordHash = passwordDetails[1],
            PasswordSalt = passwordDetails[2],
            Role = user.Role
        };
        
        await _userService.AddAsync(userDto);
        
        UserEmailRolePasswordDto? userPassword = _mapper.Map<UserEmailRolePasswordDto>((user, password));
        await _rabbitMqPublisher.PublishAsync(userPassword, "create-user-queue");
        
        return Ok();
    }

    /// <summary>
    /// Updates the password for the authenticated user.
    /// </summary>
    /// <param name="userPassword">The new password.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize]
    [HttpPost("updatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UserParamDto userPassword)
    {
        string password = userPassword.Password;
        if(password.IsNullOrEmpty()) return StatusCode(400, "Password is empty");
        if(password.Length < 8) return StatusCode(401, "Password is less than 8");
        
        string? id = User.FindFirst("userId")?.Value;
        if (id == null) return StatusCode(402, "Incorrect token!");

        User? user = await _userService.GetByIdAsync(int.Parse(id));
        if (user == null) return StatusCode(402, "User not found!");

        byte[][] passwordDetails = _authHelper.UpdatePassword(user, password);
        bool result = await _userService.UpdatePasswordAsync(user.Email, passwordDetails[0], passwordDetails[1]);
        if (result) return Ok();
        return BadRequest("Password was not updated!");
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize(Roles = "ADMIN")]
    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (await _userService.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}