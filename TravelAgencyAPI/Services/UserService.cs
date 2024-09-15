using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Services.Interfaces;
using TravelAgencyAPI.Services.ModelServiceInterfaces;

namespace TravelAgencyAPI.Services;

public class UserService : IRepository<User, UserDto>, IUserService
{
    private TravelDbContext _context;
    private readonly IMapper _mapper;

    public UserService(TravelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<User?> GetByIdWithIncludeAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<int> AddAsync(UserDto userDto)
    {
        User user = _mapper.Map<User>(userDto);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task<bool> UpdateAsync(UserDto userDto)
    {
        User? user = await _context.Users.FindAsync(userDto.Id);
        if (user == null) return false;
        
        user.Email = userDto.Email ?? user.Email;
        user.Role = userDto.Role ?? user.Role;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        User? user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UpdatePasswordAsync(string email, byte[] passwordHash, byte[] passwordSalt)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;
        
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await _context.SaveChangesAsync();

        return true;
    }

    
    public async Task<bool> CreateReservePasswordAsync(string email, byte[] reservePasswordHash, byte[] reservePasswordSalt)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;
        
        user.ReservePasswordHash = reservePasswordHash;
        user.ReservePasswordSalt = reservePasswordSalt;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveReservePassword(string email)
    {
        User? user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null) return false;
        
        user.ReservePasswordHash = null;
        user.ReservePasswordSalt = null;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsUsedEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email) != null;
    }
}