using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Shared.DTOs.Payment;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> GetPaymentAsync(Guid id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByCampaignAsync(Guid campaignId);
        Task<IEnumerable<PaymentDto>> GetPaymentsBySenderAsync(Guid senderId);
        Task<IEnumerable<PaymentDto>> GetPaymentsByRecipientAsync(Guid recipientId);
        Task<PaymentDto> CreatePaymentAsync(Guid senderId, CreatePaymentRequest request);
        Task<bool> UpdatePaymentStatusAsync(Guid paymentId, string status);
        Task<bool> ProcessEscrowPaymentAsync(Guid campaignId, decimal amount);
        Task<bool> ReleaseEscrowPaymentAsync(Guid paymentId);
    }
}