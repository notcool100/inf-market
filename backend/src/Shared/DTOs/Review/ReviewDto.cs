using System;

namespace InfluencerMarketplace.Shared.DTOs.Review
{
    /// <summary>
    /// Review data transfer object
    /// </summary>
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public Guid ReviewerId { get; set; }
        public Guid InfluencerProfileId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

