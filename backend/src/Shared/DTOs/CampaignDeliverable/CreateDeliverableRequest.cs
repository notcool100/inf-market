using System;
using System.ComponentModel.DataAnnotations;

namespace InfluencerMarketplace.Shared.DTOs.CampaignDeliverable
{
    /// <summary>
    /// Request model for creating a campaign deliverable
    /// </summary>
    public class CreateDeliverableRequest
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string DeliverableType { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}

