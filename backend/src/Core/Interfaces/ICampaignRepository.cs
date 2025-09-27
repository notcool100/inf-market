using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
        Task<IEnumerable<Campaign>> GetCampaignsByBrandIdAsync(Guid brandId);
        Task<IEnumerable<Campaign>> GetCampaignsByInfluencerIdAsync(Guid influencerId);
        Task<bool> AssignInfluencerAsync(Guid campaignId, Guid influencerId);
        Task<bool> UpdateCampaignStatusAsync(Guid campaignId, CampaignStatus status);
        Task<IEnumerable<Campaign>> GetAvailableCampaignsAsync();
    }
}