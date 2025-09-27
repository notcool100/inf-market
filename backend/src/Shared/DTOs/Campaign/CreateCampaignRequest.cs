using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfluencerMarketplace.Shared.DTOs.Campaign
{
    public class CreateCampaignRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Requirements { get; set; }

        [Required]
        public List<string> Deliverables { get; set; }

        public Dictionary<string, object> TargetAudience { get; set; }

        [Required]
        public List<string> TargetPlatforms { get; set; }
    }
}