using System;

namespace InfluencerMarketplace.Core.Models
{
    public enum DeliverableStatus
    {
        Pending,
        Submitted,
        Approved,
        Rejected
    }

    public class CampaignDeliverable
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DeliverableType { get; set; } // e.g., "Instagram Post", "TikTok Video", etc.
        public string ProofUrl { get; set; } // URL to the post or content
        public string ScreenshotUrl { get; set; } // URL to screenshot of the content
        public string FeedbackNotes { get; set; }
        public DeliverableStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}