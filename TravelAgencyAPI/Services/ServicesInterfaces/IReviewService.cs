using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IReviewService
{
    public Task<Review?> GetUserReview(int userId, int tourId);
    public Task<List<Review>> GetTourReviews(int tourId);
    public Task<bool> DeleteUserReviewAsync(int userId, int tourId);
}