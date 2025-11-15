using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByInfluencerIdAsync(Guid influencerProfileId);
        Task<Review> GetReviewByCampaignIdAsync(Guid campaignId);
    }
}

