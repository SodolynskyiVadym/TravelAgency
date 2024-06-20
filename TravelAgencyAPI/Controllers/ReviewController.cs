using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ReviewRepository _reviewRepository;
    
    public ReviewController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _reviewRepository = new ReviewRepository(context, mapper, redis);
    }
    
    [Authorize]
    [HttpGet("getUserReview/{tourId}")]
    public async Task<Review?> GetReview(int tourId)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        return await _reviewRepository.GetUserReview(userId, tourId);
    }
    
    [HttpGet("getTourReviews/{tourId}")]
    public async Task<List<Review>> GetAllReviews(int tourId)
    {
        return await _reviewRepository.GetTourReviews(tourId);
    }
    
    [HttpGet("getAllReviews")]
    public async Task<List<Review>> GetAllReviews()
    {
        return await _reviewRepository.GetAllAsync();
    }
    
    [Authorize]
    [HttpPost("createReview")]
    public async Task<IActionResult> CreateReview(ReviewDto review)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        if(userId != review.UserId) return Unauthorized("You can only create reviews for yourself");
        if (review.Rating <= 0 || review.Rating > 5) return BadRequest("Rating must be between 1 and 5");

        review.UserId = userId;
        if (await _reviewRepository.AddAsync(review) != 0) return Ok(review);
        return BadRequest();
    }
    
    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateReview(int id, ReviewDto review)
    {
        if (review.Rating <= 0 || review.Rating > 5) return BadRequest("Rating must be between 1 and 5");
        if (await _reviewRepository.UpdateAsync(id, review)) return Ok();
        return NoContent();
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        if (await _reviewRepository.DeleteAsync(id)) return Ok();
        return NoContent();
    }
}