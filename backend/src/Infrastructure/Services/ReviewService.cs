using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;
using InfluencerMarketplace.Shared.DTOs.Review;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IInfluencerProfileRepository _influencerProfileRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            ICampaignRepository campaignRepository,
            IInfluencerProfileRepository influencerProfileRepository)
        {
            _reviewRepository = reviewRepository;
            _campaignRepository = campaignRepository;
            _influencerProfileRepository = influencerProfileRepository;
        }

        public async Task<ReviewDto> GetReviewAsync(Guid id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return null;

            return MapToDto(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByInfluencerIdAsync(Guid influencerProfileId)
        {
            var reviews = await _reviewRepository.GetReviewsByInfluencerIdAsync(influencerProfileId);
            return reviews.Select(MapToDto);
        }

        public async Task<ReviewDto> GetReviewByCampaignIdAsync(Guid campaignId)
        {
            var review = await _reviewRepository.GetReviewByCampaignIdAsync(campaignId);
            if (review == null)
                return null;

            return MapToDto(review);
        }

        public async Task<ReviewDto> CreateReviewAsync(Guid campaignId, Guid reviewerId, CreateReviewRequest request)
        {
            // Check if review already exists for this campaign
            var existingReview = await _reviewRepository.GetReviewByCampaignIdAsync(campaignId);
            if (existingReview != null)
                throw new InvalidOperationException("A review already exists for this campaign");

            // Verify campaign exists and is completed
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
                throw new ArgumentException("Campaign not found");

            if (campaign.Status != CampaignStatus.Completed)
                throw new InvalidOperationException("Can only review completed campaigns");

            // Get influencer profile from campaign
            if (!campaign.InfluencerId.HasValue)
                throw new InvalidOperationException("Campaign has no assigned influencer");

            var influencerProfiles = await _influencerProfileRepository.FindAsync(
                "SELECT * FROM InfluencerProfiles WHERE UserId = @UserId",
                new { UserId = campaign.InfluencerId.Value });

            var influencerProfile = influencerProfiles.FirstOrDefault();
            if (influencerProfile == null)
                throw new ArgumentException("Influencer profile not found");

            var review = new Review
            {
                CampaignId = campaignId,
                ReviewerId = reviewerId,
                InfluencerProfileId = influencerProfile.Id,
                Rating = request.Rating,
                Comment = request.Comment,
                IsPublic = request.IsPublic
            };

            var id = await _reviewRepository.CreateAsync(review);

            // Update influencer profile statistics
            await UpdateInfluencerRatingAsync(influencerProfile.Id);

            return await GetReviewAsync(id);
        }

        public async Task<ReviewDto> UpdateReviewAsync(Guid id, Guid reviewerId, CreateReviewRequest request)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                throw new ArgumentException("Review not found");

            if (review.ReviewerId != reviewerId)
                throw new UnauthorizedAccessException("You can only update your own reviews");

            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.IsPublic = request.IsPublic;

            await _reviewRepository.UpdateAsync(review);

            // Update influencer profile statistics
            await UpdateInfluencerRatingAsync(review.InfluencerProfileId);

            return await GetReviewAsync(id);
        }

        public async Task<bool> DeleteReviewAsync(Guid id, Guid reviewerId)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return false;

            if (review.ReviewerId != reviewerId)
                throw new UnauthorizedAccessException("You can only delete your own reviews");

            var influencerProfileId = review.InfluencerProfileId;
            var result = await _reviewRepository.DeleteAsync(id);

            if (result)
            {
                // Update influencer profile statistics
                await UpdateInfluencerRatingAsync(influencerProfileId);
            }

            return result;
        }

        private async Task UpdateInfluencerRatingAsync(Guid influencerProfileId)
        {
            var reviews = await _reviewRepository.GetReviewsByInfluencerIdAsync(influencerProfileId);
            var reviewsList = reviews.ToList();

            if (reviewsList.Count > 0)
            {
                var averageRating = reviewsList.Average(r => r.Rating);
                var completedCampaigns = reviewsList.Count;

                var influencerProfiles = await _influencerProfileRepository.FindAsync(
                    "SELECT * FROM InfluencerProfiles WHERE Id = @Id",
                    new { Id = influencerProfileId });

                var profile = influencerProfiles.FirstOrDefault();
                if (profile != null)
                {
                    profile.AverageRating = averageRating;
                    profile.CompletedCampaigns = completedCampaigns;
                    await _influencerProfileRepository.UpdateAsync(profile);
                }
            }
        }

        private ReviewDto MapToDto(Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                CampaignId = review.CampaignId,
                ReviewerId = review.ReviewerId,
                InfluencerProfileId = review.InfluencerProfileId,
                Rating = review.Rating,
                Comment = review.Comment,
                IsPublic = review.IsPublic,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };
        }
    }
}

