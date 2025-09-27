using System;

namespace InfluencerMarketplace.Shared.DTOs.Payment
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid? CampaignId { get; set; }
        public string CampaignTitle { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; }
        public Guid RecipientId { get; set; }
        public string RecipientName { get; set; }
        public decimal Amount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string TransactionReference { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}