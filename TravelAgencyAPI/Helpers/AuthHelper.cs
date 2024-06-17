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
using TravelAgencyAPI.Settings;

namespace TravelAgencyAPI.Helpers;

public class AuthHelper
{
    private readonly UserRepository _userRepository;
    private readonly AuthSetting _authSetting;
    private readonly MailHelper _mailHelper;
    private readonly string possibleValuePassword = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    public AuthHelper(IOptions<AuthSetting> authSetting, IOptions<MailSetting> mailSetting, UserRepository userRepository)
    {
        _userRepository = userRepository;
        _mailHelper = new MailHelper(mailSetting);
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


    public async Task<bool> CreateUser(UserCreateDto user)
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
        
        // if(await _userRepository.AddAsync(userDto))
        // {
            _mailHelper.SendPassword(user.Email, password, user.Role);
            return true;
        // }
        // return false;
    }


    public async Task<bool> UpdatePassword(User userUpdate, string password)
    {
        byte[] passwordSalt = new byte[128 / 8];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        byte[] passwordHash = GetPasswordHash(password, passwordSalt);

        await _userRepository.UpdatePasswordAsync(userUpdate.Email, passwordHash, passwordSalt);

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