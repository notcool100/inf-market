using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Shared.DTOs.Campaign;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface ICampaignService
    {
        Task<CampaignDto> GetCampaignAsync(Guid id);
        Task<IEnumerable<CampaignDto>> GetCampaignsByBrandAsync(Guid brandId);
        Task<IEnumerable<CampaignDto>> GetCampaignsByInfluencerAsync(Guid influencerId);
        Task<IEnumerable<CampaignDto>> GetAvailableCampaignsAsync();
        Task<CampaignDto> CreateCampaignAsync(Guid brandId, CreateCampaignRequest request);
        Task<CampaignDto> UpdateCampaignAsync(Guid id, CreateCampaignRequest request);
        Task<bool> DeleteCampaignAsync(Guid id);
        Task<bool> AssignInfluencerToCampaignAsync(Guid campaignId, Guid influencerId);
        Task<bool> UpdateCampaignStatusAsync(Guid campaignId, string status);
        Task<IEnumerable<CampaignDto>> SearchCampaignsAsync(string status = null, decimal? minBudget = null, decimal? maxBudget = null, DateTime? startDate = null, DateTime? endDate = null, string platform = null, string niche = null);
    }
}