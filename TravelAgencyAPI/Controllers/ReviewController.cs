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
        _reviewRepository = new ReviewRepository(context, mapper);
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
    [HttpPost("create")]
    public async Task<IActionResult> CreateReview(ReviewDto review)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        if (review.Rating <= 0 || review.Rating > 5) return BadRequest("Rating must be between 1 and 5");
        if (await _reviewRepository.GetUserReview(userId, review.TourId) != null) return BadRequest("You have already reviewed this tour");
        
        review.UserId = userId;
        if (await _reviewRepository.AddAsync(review) != 0) return Ok();
        return BadRequest();
    }
    
    [Authorize]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateReview(ReviewDto review)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        if (review.Rating <= 0 || review.Rating > 5) return BadRequest("Rating must be between 1 and 5");
        if (await _reviewRepository.UpdateAsync(review)) return Ok();
        return NoContent();
    }
    
    [Authorize]
    [HttpDelete("delete/{tourId}")]
    public async Task<IActionResult> DeleteReview(int tourId)
    {
        Console.WriteLine("TourId: " + tourId);
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        
        if (await _reviewRepository.DeleteUserReviewAsync(userId, tourId)) return Ok();
        return BadRequest("You have not reviewed this tour");
    }
}