using TravelAgencyAPIServer.Models;

namespace TravelAgencyAPIServer.Services.ModelServiceInterfaces;

public interface IUserService
{
    public Task<User?> GetUserByEmail(string email);
    public Task<bool> UpdatePasswordAsync(string email, byte[] passwordHash, byte[] passwordSalt);
    public Task<bool> CreateReservePasswordAsync(string email, byte[] reservePasswordHash, byte[] reservePasswordSalt);
    public Task<bool> RemoveReservePassword(string email);
    public Task<bool> IsUsedEmail(string email);
}