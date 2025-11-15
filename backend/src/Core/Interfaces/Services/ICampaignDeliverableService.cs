using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Shared.DTOs.CampaignDeliverable;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface ICampaignDeliverableService
    {
        Task<CampaignDeliverableDto> GetDeliverableAsync(Guid id);
        Task<IEnumerable<CampaignDeliverableDto>> GetDeliverablesByCampaignIdAsync(Guid campaignId);
        Task<CampaignDeliverableDto> CreateDeliverableAsync(Guid campaignId, CreateDeliverableRequest request);
        Task<CampaignDeliverableDto> UpdateDeliverableAsync(Guid id, CreateDeliverableRequest request);
        Task<bool> SubmitDeliverableAsync(Guid id, SubmitDeliverableRequest request);
        Task<bool> ApproveDeliverableAsync(Guid id, ReviewDeliverableRequest request);
        Task<bool> RejectDeliverableAsync(Guid id, ReviewDeliverableRequest request);
    }
}

