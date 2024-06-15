using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IUserRepository
{
    public Task<User?> GetUserByEmail(string email);
    public Task<bool> UpdatePasswordAsync(int userId, byte[] passwordHash, byte[] passwordSalt);
    public Task<bool> CreateReservePasswordAsync(int userId, byte[] reservePasswordHash, byte[] reservePasswordSalt);
}