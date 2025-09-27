using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Shared.DTOs.InfluencerProfile
{
    public class InfluencerProfileDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
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
        public List<string> ContentTypes { get; set; }
        public Dictionary<string, object> Demographics { get; set; }
        public string Location { get; set; }
        public bool IsVerified { get; set; }
        public double AverageRating { get; set; }
        public int CompletedCampaigns { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}