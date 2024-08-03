using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;
using TravelAgencyAPI.Services.ServicesInterfaces;

namespace TravelAgencyAPI.Services;

public class ReviewService : IRepository<Review, ReviewDto>, IReviewService
{
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;

    public ReviewService(TravelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Review?> GetByIdWithIncludeAsync(int id)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Tour)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        Review? review = await _context.Reviews.FirstOrDefaultAsync(review => review.Id == id);
        return review;
    }

    public async Task<List<Review>> GetAllAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<int> AddAsync(ReviewDto reviewDto)
    {
        Review review = _mapper.Map<Review>(reviewDto);
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
        return review.Id;
    }

    public async Task<bool> UpdateAsync(ReviewDto reviewUpdate)
    {
        Review? review = await _context.Reviews.FindAsync(reviewUpdate.Id);
        if (review == null) return false;
        
        review.Text = reviewUpdate.Text ?? review.Text;
        review.Rating = reviewUpdate.Rating;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Review? review = await _context.Reviews.FindAsync(id);
        if (review == null) return false;

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsUsedUniqueAttributes(ReviewDto entity)
    {
        Review? review = await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == entity.UserId && r.TourId == entity.TourId);
        if(review == null) return false;
        return review.Id != entity.Id;
    }

    public async Task<Review?> GetUserReview(int userId, int tourId)
    {
        Review? review = await _context.Reviews.FirstOrDefaultAsync(review => review.UserId == userId && review.TourId == tourId);
        return review;
    }

    public Task<List<Review>> GetTourReviews(int tourId)
    {
        return _context.Reviews.Where(r => r.TourId == tourId).ToListAsync();
    }

    public async Task<bool> DeleteUserReviewAsync(int userId, int tourId)
    {
        Review? review = _context.Reviews.FirstOrDefault(r => r.UserId == userId && r.TourId == tourId);

        if (review == null) return false;
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }
}