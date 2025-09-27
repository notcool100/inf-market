using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByCampaignIdAsync(Guid campaignId);
        Task<IEnumerable<Payment>> GetPaymentsBySenderIdAsync(Guid senderId);
        Task<IEnumerable<Payment>> GetPaymentsByRecipientIdAsync(Guid recipientId);
        Task<bool> UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status);
    }
}