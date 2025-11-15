using System.ComponentModel.DataAnnotations;

namespace InfluencerMarketplace.Shared.DTOs.CampaignDeliverable
{
    /// <summary>
    /// Request model for reviewing (approving/rejecting) a deliverable
    /// </summary>
    public class ReviewDeliverableRequest
    {
        [Required]
        public string Status { get; set; } // "Approved" or "Rejected"

        public string FeedbackNotes { get; set; }
    }
}

