using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Helpers;

public class AuthHelper
{
    private readonly UserRepository _userRepository;
    private readonly string _passwordKey;
    private readonly string _tokenKey;
    private readonly string possibleValuePassword = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    public AuthHelper(IConfiguration config, UserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordKey = config.GetSection("AppSettings:PasswordKey").Value ?? throw new InvalidOperationException();
        _tokenKey = config.GetSection("AppSettings:TokenKey").Value ?? throw new InvalidOperationException();
    }

    private byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString = _passwordKey + Convert.ToBase64String(passwordSalt);

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


    public string CreateToken(int userId, string email, string role)
    {
        Claim[] claims = new Claim[]
        {
            new Claim("userId", userId.ToString()),
            new Claim("email", email),
            new Claim("role", role)
        };

        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey));

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


    public async Task<bool> RegisterUser(UserLoginRegistrationDto userRegistration, string userRole)
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
            Role = userRole,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        await _userRepository.AddAsync(user);
        return true;
    }


    public async Task<bool> UpdatePassword(int userId, UserLoginRegistrationDto userUpdate)
    {
        if (userUpdate.Password.IsNullOrEmpty()) return false;

        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        byte[] passwordHash = GetPasswordHash(userUpdate.Password, passwordSalt);

        await _userRepository.UpdatePasswordAsync(userId, passwordHash, passwordSalt);

        return true;
    }
    

    public async Task<string> CreateReservePassword(int userId)
    {
        string password = this.GenerateRandomPassword();
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }
        byte[] passwordHash = GetPasswordHash(password, passwordSalt);

        await _userRepository.CreateReservePasswordAsync(userId, passwordHash, passwordSalt);

        return password;
    }



}