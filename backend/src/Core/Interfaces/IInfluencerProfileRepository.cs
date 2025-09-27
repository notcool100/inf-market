using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface IInfluencerProfileRepository : IRepository<InfluencerProfile>
    {
        Task<InfluencerProfile> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<InfluencerProfile>> SearchInfluencersAsync(string nicheFocus, string location, int? minFollowers, decimal? maxRate);
        Task<bool> UpdateRatingAsync(Guid influencerProfileId, double newAverageRating);
        Task<bool> IncrementCompletedCampaignsAsync(Guid influencerProfileId);
    }
}