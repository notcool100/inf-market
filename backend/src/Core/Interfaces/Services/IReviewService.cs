using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Shared.DTOs.Review;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> GetReviewAsync(Guid id);
        Task<IEnumerable<ReviewDto>> GetReviewsByInfluencerIdAsync(Guid influencerProfileId);
        Task<ReviewDto> GetReviewByCampaignIdAsync(Guid campaignId);
        Task<ReviewDto> CreateReviewAsync(Guid campaignId, Guid reviewerId, CreateReviewRequest request);
        Task<ReviewDto> UpdateReviewAsync(Guid id, Guid reviewerId, CreateReviewRequest request);
        Task<bool> DeleteReviewAsync(Guid id, Guid reviewerId);
    }
}

