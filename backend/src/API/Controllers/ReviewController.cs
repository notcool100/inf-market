using System;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Shared.DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Review management endpoints for brands to review influencers
    /// </summary>
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Get all reviews for an influencer
        /// </summary>
        /// <param name="influencerId">Influencer profile unique identifier</param>
        /// <returns>List of reviews</returns>
        /// <response code="200">Returns the list of reviews</response>
        [HttpGet("influencers/{influencerId}/reviews")]
        [ProducesResponseType(typeof(System.Collections.Generic.IEnumerable<ReviewDto>), 200)]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<ReviewDto>>> GetInfluencerReviews(Guid influencerId)
        {
            var reviews = await _reviewService.GetReviewsByInfluencerIdAsync(influencerId);
            return Ok(reviews);
        }

        /// <summary>
        /// Get review for a specific campaign
        /// </summary>
        /// <param name="campaignId">Campaign unique identifier</param>
        /// <returns>Review details</returns>
        /// <response code="200">Returns the review</response>
        /// <response code="404">Review not found</response>
        [HttpGet("campaigns/{campaignId}/review")]
        [ProducesResponseType(typeof(ReviewDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ReviewDto>> GetCampaignReview(Guid campaignId)
        {
            var review = await _reviewService.GetReviewByCampaignIdAsync(campaignId);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        /// <summary>
        /// Create a review for a campaign (Brand only)
        /// </summary>
        /// <param name="campaignId">Campaign unique identifier</param>
        /// <param name="request">Review details</param>
        /// <returns>Created review</returns>
        /// <response code="201">Review created successfully</response>
        /// <response code="400">Invalid review data or campaign not completed</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not a Brand</response>
        [Authorize(Roles = "Brand")]
        [HttpPost("campaigns/{campaignId}/review")]
        [ProducesResponseType(typeof(ReviewDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ReviewDto>> CreateReview(Guid campaignId, CreateReviewRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var review = await _reviewService.CreateReviewAsync(campaignId, userId, request);
                return CreatedAtAction(nameof(GetCampaignReview), new { campaignId = campaignId }, review);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a review (Brand only)
        /// </summary>
        /// <param name="id">Review unique identifier</param>
        /// <param name="request">Updated review details</param>
        /// <returns>Updated review</returns>
        /// <response code="200">Review updated successfully</response>
        /// <response code="400">Invalid review data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not a Brand or not the review owner</response>
        /// <response code="404">Review not found</response>
        [Authorize(Roles = "Brand")]
        [HttpPut("reviews/{id}")]
        [ProducesResponseType(typeof(ReviewDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ReviewDto>> UpdateReview(Guid id, CreateReviewRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var review = await _reviewService.UpdateReviewAsync(id, userId, request);
                return Ok(review);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        /// <summary>
        /// Delete a review (Brand only)
        /// </summary>
        /// <param name="id">Review unique identifier</param>
        /// <returns>No content</returns>
        /// <response code="204">Review deleted successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not a Brand or not the review owner</response>
        /// <response code="404">Review not found</response>
        [Authorize(Roles = "Brand")]
        [HttpDelete("reviews/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteReview(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _reviewService.DeleteReviewAsync(id, userId);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}

