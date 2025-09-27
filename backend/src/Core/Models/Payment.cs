using System;

namespace InfluencerMarketplace.Core.Models
{
    public enum PaymentStatus
    {
        Pending,
        InEscrow,
        Released,
        Refunded,
        Failed
    }

    public enum PaymentType
    {
        CampaignDeposit,
        CampaignPayout,
        WalletDeposit,
        WalletWithdrawal,
        CommissionFee
    }

    public class Payment
    {
        public Guid Id { get; set; }
        public Guid? CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public Guid SenderId { get; set; } // User ID of sender
        public User Sender { get; set; }
        public Guid RecipientId { get; set; } // User ID of recipient
        public User Recipient { get; set; }
        public decimal Amount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal NetAmount { get; set; } // Amount after commission
        public string Currency { get; set; } // Default "NPR"
        public PaymentStatus Status { get; set; }
        public PaymentType Type { get; set; }
        public string TransactionReference { get; set; } // External payment reference
        public string PaymentMethod { get; set; } // e.g., "eSewa", "Khalti", "Wallet"
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}