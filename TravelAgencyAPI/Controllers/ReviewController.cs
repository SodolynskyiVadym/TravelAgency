using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TravelAgencyAPI.DTO;
using TravelAgencyAPI.Helpers;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Repositories;
using TravelAgencyAPI.Services;

namespace TravelAgencyAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;
    
    public ReviewController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _reviewService = new ReviewService(context, mapper);
    }
    
    [Authorize]
    [HttpGet("getUserReview/{tourId}")]
    public async Task<Review?> GetReview(int tourId)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        return await _reviewService.GetUserReview(userId, tourId);
    }
    
    [HttpGet("getTourReviews/{tourId}")]
    public async Task<List<Review>> GetAllReviews(int tourId)
    {
        return await _reviewService.GetTourReviews(tourId);
    }
    
    [HttpGet("getAllReviews")]
    public async Task<List<Review>> GetAllReviews()
    {
        return await _reviewService.GetAllAsync();
    }
    
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateReview(ReviewDto review)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        if (review.Rating <= 0 || review.Rating > 5) return BadRequest("Rating must be between 1 and 5");
        if(await _reviewService.IsUsedUniqueAttributes(review)) return BadRequest("Place already exists!");
        
        review.UserId = userId;
        if (await _reviewService.AddAsync(review) != 0) return Ok();
        return BadRequest();
    }
    
    [Authorize]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateReview(ReviewDto review)
    {
        if(await _reviewService.IsUsedUniqueAttributes(review)) return BadRequest("Place already exists!");
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        if (review.Rating <= 0 || review.Rating > 5) return BadRequest("Rating must be between 1 and 5");
        if (await _reviewService.UpdateAsync(review)) return Ok();
        return NoContent();
    }
    
    [Authorize]
    [HttpDelete("delete/{tourId}")]
    public async Task<IActionResult> DeleteReview(int tourId)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        
        if (await _reviewService.DeleteUserReviewAsync(userId, tourId)) return Ok();
        return BadRequest("You have not reviewed this tour");
    }
}