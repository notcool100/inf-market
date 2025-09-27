using System;

namespace InfluencerMarketplace.Core.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public Guid ReviewerId { get; set; } // User ID of the reviewer (brand)
        public User Reviewer { get; set; }
        public Guid InfluencerProfileId { get; set; }
        public InfluencerProfile InfluencerProfile { get; set; }
        public int Rating { get; set; } // 1-5 stars
        public string Comment { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}