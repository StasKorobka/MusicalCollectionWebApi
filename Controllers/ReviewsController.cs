using DataAccess.Models;
using DataAccess.Models.DTO.Review;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MusicalCollectionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewsController> _logger;
        public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }
        [HttpGet("users/{userId}/reviews")]
        public async Task<IActionResult> GetAllReviews(int userId)
        {
            try
            {
                var reviews = await _reviewService.GetAllReviewsAsync(userId);
                _logger.LogInformation($"User({userId}) got all reviews");
                return Ok(reviews);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: user({userId}) not found");
                return NotFound(ex.Message);
            }
        }
        [HttpPost("users/{userId}/reviews")]
        public async Task<ActionResult<ReviewDto>> AddReview(int userId, [FromBody] AddReviewDto addReviewDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error: Review is invalid");
                return BadRequest(ModelState);
            }

            try
            {
                var review = await _reviewService.AddReviewAsync(userId, addReviewDto);
                _logger.LogInformation($"User({userId}) added new review");
                return CreatedAtAction(nameof(AddReview), 
                    new { userId, reviewId = review.ReviewId },
                    review);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: Review or User is not found");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error: Review is invalid");
                return Conflict(ex.Message);
            }
        }

        [HttpPut("users/{userId}/reviews/{reviewId}")]
        public async Task<ActionResult<ReviewDto>> EditReview(int reviewId, int userId, [FromBody] EditReviewDto editReviewDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error: Review is invalid");
                return BadRequest(ModelState);
            }

            try
            {
                var review = await _reviewService.EditReviewAsync(reviewId, userId, editReviewDto);
                _logger.LogInformation($"User({userId}) edited review({reviewId})");
                return Ok(review);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: Review or User is not found");
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("users/{userId}/reviews/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId, int userId)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(reviewId, userId);
                _logger.LogInformation($"User({userId}) deleted review({reviewId})");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: Review or User is not found");
                return NotFound(ex.Message);
            }
        }
    }
}