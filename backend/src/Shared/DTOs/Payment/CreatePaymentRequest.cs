using System;
using System.ComponentModel.DataAnnotations;

namespace InfluencerMarketplace.Shared.DTOs.Payment
{
    public class CreatePaymentRequest
    {
        public Guid? CampaignId { get; set; }

        [Required]
        public Guid RecipientId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "NPR";

        [Required]
        public string PaymentMethod { get; set; }

        public string Notes { get; set; }
    }
}