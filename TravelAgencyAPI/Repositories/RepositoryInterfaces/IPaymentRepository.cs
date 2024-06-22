using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IPaymentRepository
{
    public Task<Payment?> GetByUserIdTourId(int userId, int tourId);
    public Task<List<Payment>> GetByUserId(int userId);
    public Task DeleteUnpaid();
}