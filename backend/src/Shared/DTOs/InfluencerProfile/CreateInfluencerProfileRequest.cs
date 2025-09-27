using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfluencerMarketplace.Shared.DTOs.InfluencerProfile
{
    public class CreateInfluencerProfileRequest
    {
        [Required]
        public string Bio { get; set; }

        [Required]
        public string NicheFocus { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int FollowersCount { get; set; }

        public string InstagramHandle { get; set; }
        public string TikTokHandle { get; set; }
        public string YouTubeChannel { get; set; }
        public string FacebookPage { get; set; }
        public string LinkedInProfile { get; set; }
        public string WebsiteUrl { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MinCampaignRate { get; set; }

        [Required]
        public List<string> ContentTypes { get; set; }

        public Dictionary<string, object> Demographics { get; set; }

        [Required]
        public string Location { get; set; }
    }
}