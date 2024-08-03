﻿using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Services.ServicesInterfaces;

public interface IReviewService
{
    public Task<Review?> GetUserReview(int userId, int tourId);
    public Task<List<Review>> GetTourReviews(int tourId);
    public Task<bool> DeleteUserReviewAsync(int userId, int tourId);
}