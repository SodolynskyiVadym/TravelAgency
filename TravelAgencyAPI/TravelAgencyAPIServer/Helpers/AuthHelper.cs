using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services;
using TravelAgencyAPIServer.Settings;

namespace TravelAgencyAPIServer.Helpers;

public class AuthHelper
{
    private readonly AuthSetting _authSetting;
    private readonly string possibleValuePassword = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    
    public AuthHelper(AuthSetting authSetting)
    {
        _authSetting = authSetting;
    }

    private byte[] GetPasswordHash(string password, byte[] passwordSalt)
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
    
    public string GenerateRandomPassword()
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
        Claim[] claims =
        [
            new("userId", user.Id.ToString()),
            new("email", user.Email),
            new("role", user.Role)
        ];

        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSetting.TokenKey));

        SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }


    public UserDto EncryptUserPassword(UserEmailPasswordDto userRegistration, string role)
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
            Role = role,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        return user;
    }


    public byte[][] UpdatePassword(User userUpdate, string password)
    {
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        byte[] passwordHash = GetPasswordHash(password, passwordSalt);
        userUpdate.PasswordHash = passwordHash;
        userUpdate.PasswordSalt = passwordSalt;
        return [passwordHash, passwordSalt];
    }
    

    public byte[][] CreateReservePassword(string password)
    {
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }
        
        byte[] passwordHash = GetPasswordHash(password, passwordSalt);

        return [passwordHash, passwordSalt];
    }
    
    
    public bool CheckPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        byte[] passwordHashToCheck = GetPasswordHash(password, passwordSalt);
        for (var index = 0; index < passwordHashToCheck.Length; index++) if (passwordHashToCheck[index] != passwordHash[index]) return false;
        return true;
    }

    public static bool IsEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}