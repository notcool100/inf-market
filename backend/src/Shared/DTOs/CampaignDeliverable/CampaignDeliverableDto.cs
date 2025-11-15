using System;

namespace InfluencerMarketplace.Shared.DTOs.CampaignDeliverable
{
    /// <summary>
    /// Campaign deliverable data transfer object
    /// </summary>
    public class CampaignDeliverableDto
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DeliverableType { get; set; }
        public string ProofUrl { get; set; }
        public string ScreenshotUrl { get; set; }
        public string FeedbackNotes { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

