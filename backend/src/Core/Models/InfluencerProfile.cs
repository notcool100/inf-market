using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Core.Models
{
    public class InfluencerProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Bio { get; set; }
        public string NicheFocus { get; set; }
        public int FollowersCount { get; set; }
        public string InstagramHandle { get; set; }
        public string TikTokHandle { get; set; }
        public string YouTubeChannel { get; set; }
        public string FacebookPage { get; set; }
        public string LinkedInProfile { get; set; }
        public string WebsiteUrl { get; set; }
        public decimal MinCampaignRate { get; set; }
        public string ContentTypes { get; set; } // Stored as JSON array
        public string Demographics { get; set; } // Stored as JSON object
        public string Location { get; set; }
        public bool IsVerified { get; set; }
        public double AverageRating { get; set; }
        public int CompletedCampaigns { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Campaign> AssignedCampaigns { get; set; } = new List<Campaign>();
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}