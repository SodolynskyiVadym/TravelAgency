using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Newtonsoft.Json;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories.RepositoryInterfaces;

namespace TravelAgencyAPI.Repositories;

public class ReviewRepository : IRepository<Review, ReviewDto>, IReviewRepository
{
    private readonly TravelDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDatabase _redis;

    public ReviewRepository(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redisConnection)
    {
        _context = context;
        _mapper = mapper;
        _redis = redisConnection.GetDatabase();
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        string redisKey = "review" + id;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            string jsonData = await _redis.StringGetAsync(redisKey);
            return JsonConvert.DeserializeObject<Review>(jsonData);
        }
        Review? review = await _context.Reviews.FirstOrDefaultAsync(review => review.Id == id);
        if(review != null) 
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(review));
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

    public async Task<bool> UpdateAsync(int id, ReviewDto reviewUpdate)
    {
        Review? review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        if (review == null) return false;
        
        review.Text = reviewUpdate.Text ?? review.Text;
        review.Rating = review.Rating;
        
        await _context.SaveChangesAsync();
        
        string redisKey = "review" + id;
        if(await _redis.KeyExistsAsync(redisKey))
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(review));
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Review? review = await _context.Reviews.FindAsync(id);
        if (review == null) return false;

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        await _redis.KeyDeleteAsync("review" + id);
        return true;
    }

    public async Task<Review?> GetUserReview(int userId, int tourId)
    {
        string redisKey =  userId + "userReview" + tourId;
        if (await _redis.KeyExistsAsync(redisKey))
        {
            string jsonData = await _redis.StringGetAsync(redisKey);
            return JsonConvert.DeserializeObject<Review>(jsonData);
        }
        Review? review = await _context.Reviews.FirstOrDefaultAsync(review => review.UserId == userId && review.TourId == tourId);
        if(review != null) 
            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(review));
        return review;
    }

    public Task<List<Review>> GetTourReviews(int tourId)
    {
        return _context.Reviews.Where(r => r.TourId == tourId).ToListAsync();
    }
}