using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;
using TravelAgencyAPI.Services;
using TravelAgencyAPI.Settings;

namespace TravelAgencyAPI.Helpers;

public class AuthHelper
{
    private readonly UserService _userService;
    private readonly AuthSetting _authSetting;
    private readonly string possibleValuePassword = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    public AuthHelper(IOptions<AuthSetting> authSetting, UserService userService)
    {
        _userService = userService;
        _authSetting = authSetting.Value;
    }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString = _authSetting.PasswordKey + Convert.ToBase64String(passwordSalt);

        return KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000000,
            numBytesRequested: 256 / 8
        );
    }
    
    private string GenerateRandomPassword()
    {
        int length = 12;
        var randomBytes = new byte[length];

        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }

        var result = new StringBuilder(length);

        foreach (var b in randomBytes)
        {
            result.Append(possibleValuePassword[b % (possibleValuePassword.Length)]);
        }

        return result.ToString();
    }


    public string CreateToken(User user)
    {
        Claim[] claims = new Claim[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("email", user.Email),
            new Claim("role", user.Role)
        };

        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSetting.TokenKey));

        SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }


    public async Task<int> RegisterUser(UserLoginRegistrationDto userRegistration)
    {
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        byte[] passwordHash = GetPasswordHash(userRegistration.Password, passwordSalt);
        
        UserDto user = new UserDto()
        {
            Email = userRegistration.Email,
            Role = "USER",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        return await _userService.AddAsync(user);
    }


    public async Task<string> RegisterEditorAdmin(UserEmailRoleDto user)
    {
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }
        
        string password = this.GenerateRandomPassword();

        byte[] passwordHash = GetPasswordHash(password, passwordSalt);
        
        UserDto userDto = new UserDto()
        {
            Email = user.Email,
            Role = user.Role,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        
        if((await _userService.AddAsync(userDto)) > 0) return password;
        return string.Empty;
    }


    public async Task<bool> UpdatePassword(User userUpdate, string password)
    {
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        byte[] passwordHash = GetPasswordHash(password, passwordSalt);

        await _userService.UpdatePasswordAsync(userUpdate.Email, passwordHash, passwordSalt);

        return true;
    }
    

    public async Task<string> CreateReservePassword(string email)
    {
        string password = this.GenerateRandomPassword();
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }
        
        byte[] passwordHash = GetPasswordHash(password, passwordSalt);

        await _userService.CreateReservePasswordAsync(email, passwordHash, passwordSalt);

        return password;
    }
    
    
    public bool CheckPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        byte[] passwordHashToCheck = GetPasswordHash(password, passwordSalt);
        for (var index = 0; index < passwordHashToCheck.Length; index++) if (passwordHashToCheck[index] != passwordHash[index]) return false;
        return true;
    }
}