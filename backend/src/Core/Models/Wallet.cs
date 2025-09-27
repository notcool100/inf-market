using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Core.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } // Default "NPR"
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        CampaignPayment,
        CampaignEarning,
        CommissionFee,
        Refund
    }

    public class WalletTransaction
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public Guid? PaymentId { get; set; }
        public Payment Payment { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}