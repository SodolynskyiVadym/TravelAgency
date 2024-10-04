using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TravelAgencyAPIServer.DTO;
using TravelAgencyAPIServer.Helpers;
using TravelAgencyAPIServer.Models;
using TravelAgencyAPIServer.Services;

namespace TravelAgencyAPIServer.Controllers;

/// <summary>
/// The ReviewController class handles review-related operations for the Travel Agency API.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;

    /// <summary>
    /// Initializes a new instance of the ReviewController class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="mapper">The object mapper.</param>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public ReviewController(TravelDbContext context, IMapper mapper, IConnectionMultiplexer redis)
    {
        _reviewService = new ReviewService(context, mapper);
    }

    /// <summary>
    /// Retrieves the review of the authenticated user for a specific tour.
    /// </summary>
    /// <param name="tourId">The ID of the tour.</param>
    /// <returns>The review of the user for the specified tour.</returns>
    [Authorize]
    [HttpGet("getUserReview/{tourId}")]
    public async Task<Review?> GetReview(int tourId)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        return await _reviewService.GetUserReview(userId, tourId);
    }

    /// <summary>
    /// Retrieves all reviews for a specific tour.
    /// </summary>
    /// <param name="tourId">The ID of the tour.</param>
    /// <returns>A list of reviews for the specified tour.</returns>
    [HttpGet("getTourReviews/{tourId}")]
    public async Task<List<Review>> GetAllReviews(int tourId)
    {
        return await _reviewService.GetTourReviews(tourId);
    }

    /// <summary>
    /// Retrieves all reviews.
    /// </summary>
    /// <returns>A list of all reviews.</returns>
    [HttpGet("getAllReviews")]
    public async Task<List<Review>> GetAllReviews()
    {
        return await _reviewService.GetAllAsync();
    }

    /// <summary>
    /// Creates a new review for a tour.
    /// </summary>
    /// <param name="review">The review details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateReview(ReviewDto review)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        if (review.Rating <= 0 || review.Rating > 5) return BadRequest("Rating must be between 1 and 5");

        review.UserId = userId;
        if (await _reviewService.AddAsync(review) != 0) return Ok();
        return BadRequest();
    }

    /// <summary>
    /// Updates an existing review.
    /// </summary>
    /// <param name="review">The updated review details.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateReview(ReviewDto review)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        if (review.Rating <= 0 || review.Rating > 5) return BadRequest("Rating must be between 1 and 5");
        if (await _reviewService.UpdateAsync(review)) return Ok();
        return NoContent();
    }

    /// <summary>
    /// Deletes a review for a specific tour.
    /// </summary>
    /// <param name="tourId">The ID of the tour.</param>
    /// <returns>An action result indicating the outcome.</returns>
    [Authorize]
    [HttpDelete("delete/{tourId}")]
    public async Task<IActionResult> DeleteReview(int tourId)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value);

        if (await _reviewService.DeleteUserReviewAsync(userId, tourId)) return Ok();
        return BadRequest("You have not reviewed this tour");
    }
}