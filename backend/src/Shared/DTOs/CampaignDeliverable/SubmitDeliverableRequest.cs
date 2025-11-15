using System.ComponentModel.DataAnnotations;

namespace InfluencerMarketplace.Shared.DTOs.CampaignDeliverable
{
    /// <summary>
    /// Request model for submitting deliverable proof
    /// </summary>
    public class SubmitDeliverableRequest
    {
        [Required]
        [Url]
        public string ProofUrl { get; set; }

        [Url]
        public string ScreenshotUrl { get; set; }
    }
}

