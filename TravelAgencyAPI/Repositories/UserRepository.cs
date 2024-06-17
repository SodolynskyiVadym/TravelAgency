using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories;

public class UserRepository : IRepository<User, UserDto>, IUserRepository
{
    private TravelDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(TravelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> AddAsync(UserDto user)
    {
        await _context.Users.AddAsync(_mapper.Map<User>(user));
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, UserDto user)
    {
        await this.UpdatePasswordAsync(user.Email, user.PasswordHash, user.PasswordSalt);
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

    
    public async Task<bool> CreateReservePasswordAsync(int id, byte[] reservePasswordHash, byte[] reservePasswordSalt)
    {
        User? user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        
        user.PasswordHash = reservePasswordHash;
        user.PasswordSalt = reservePasswordSalt;
        await _context.SaveChangesAsync();

        return true;
    }
}