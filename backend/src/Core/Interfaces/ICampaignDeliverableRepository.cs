using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface ICampaignDeliverableRepository : IRepository<CampaignDeliverable>
    {
        Task<IEnumerable<CampaignDeliverable>> GetDeliverablesByCampaignIdAsync(Guid campaignId);
        Task<bool> UpdateStatusAsync(Guid id, DeliverableStatus status, string feedbackNotes = null);
    }
}

