using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Core.Models
{
    public enum CampaignStatus
    {
        Draft,
        Open,
        InProgress,
        PendingReview,
        Completed,
        Cancelled,
        Disputed
    }

    public class Campaign
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid BrandId { get; set; } // User ID of the brand
        public User Brand { get; set; }
        public Guid? InfluencerId { get; set; } // User ID of the assigned influencer
        public InfluencerProfile Influencer { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Requirements { get; set; }
        public string Deliverables { get; set; } // Stored as JSON array
        public string TargetAudience { get; set; } // Stored as JSON object
        public string TargetPlatforms { get; set; } // Stored as JSON array
        public CampaignStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CampaignDeliverable> CampaignDeliverables { get; set; } = new List<CampaignDeliverable>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public Review Review { get; set; }
    }
}