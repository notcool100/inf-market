using System;

namespace InfluencerMarketplace.Core.Models
{
    public enum NotificationType
    {
        CampaignInvitation,
        CampaignAccepted,
        CampaignRejected,
        DeliverableSubmitted,
        DeliverableApproved,
        DeliverableRejected,
        PaymentReceived,
        PaymentReleased,
        ReviewReceived,
        SystemNotification
    }

    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public string RelatedEntityType { get; set; } // e.g., "Campaign", "Payment", etc.
        public Guid? RelatedEntityId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}