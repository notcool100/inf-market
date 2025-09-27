using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Shared.DTOs.Campaign
{
    public class CampaignDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public Guid? InfluencerId { get; set; }
        public string InfluencerName { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Requirements { get; set; }
        public List<string> Deliverables { get; set; }
        public Dictionary<string, object> TargetAudience { get; set; }
        public List<string> TargetPlatforms { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}