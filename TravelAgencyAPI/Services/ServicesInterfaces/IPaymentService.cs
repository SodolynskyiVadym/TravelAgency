using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IPaymentService
{
    public Task<Payment?> GetByUserIdTourId(int userId, int tourId);
    public Task<List<Payment>> GetByUserId(int userId);
    public Task<List<Payment>> GetByTourId(int tourId);
    public Task DeleteUnpaid();
    public Task DeleteOldPayments();
}