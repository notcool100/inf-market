using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;
using InfluencerMarketplace.Shared.DTOs.Payment;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IWalletService _walletService;
        private readonly decimal _platformCommissionRate = 0.10m; // 10% commission

        public PaymentService(
            IPaymentRepository paymentRepository,
            ICampaignRepository campaignRepository,
            IWalletService walletService)
        {
            _paymentRepository = paymentRepository;
            _campaignRepository = campaignRepository;
            _walletService = walletService;
        }

        public async Task<PaymentDto> GetPaymentAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                return null;

            return MapToDto(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByCampaignAsync(Guid campaignId)
        {
            var payments = await _paymentRepository.GetPaymentsByCampaignIdAsync(campaignId);
            return MapToDtoList(payments);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsBySenderAsync(Guid senderId)
        {
            var payments = await _paymentRepository.GetPaymentsBySenderIdAsync(senderId);
            return MapToDtoList(payments);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByRecipientAsync(Guid recipientId)
        {
            var payments = await _paymentRepository.GetPaymentsByRecipientIdAsync(recipientId);
            return MapToDtoList(payments);
        }

        public async Task<PaymentDto> CreatePaymentAsync(Guid senderId, CreatePaymentRequest request)
        {
            // Calculate commission and net amount
            var commissionAmount = request.Amount * _platformCommissionRate;
            var netAmount = request.Amount - commissionAmount;

            var payment = new Payment
            {
                CampaignId = request.CampaignId,
                SenderId = senderId,
                RecipientId = request.RecipientId,
                Amount = request.Amount,
                CommissionAmount = commissionAmount,
                NetAmount = netAmount,
                Currency = request.Currency ?? "NPR",
                Status = PaymentStatus.Pending,
                Type = !string.IsNullOrEmpty(request.Type) && Enum.TryParse<PaymentType>(request.Type, out var paymentType)
                    ? paymentType
                    : PaymentType.CampaignDeposit,
                TransactionReference = request.TransactionReference,
                PaymentMethod = request.PaymentMethod,
                Notes = request.Notes
            };

            var paymentId = await _paymentRepository.CreateAsync(payment);
            payment.Id = paymentId;

            return MapToDto(payment);
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid paymentId, string status)
        {
            if (!Enum.TryParse<PaymentStatus>(status, out var paymentStatus))
                throw new ArgumentException("Invalid payment status");

            var result = await _paymentRepository.UpdatePaymentStatusAsync(paymentId, paymentStatus);
            
            if (paymentStatus == PaymentStatus.Released)
            {
                var payment = await _paymentRepository.GetByIdAsync(paymentId);
                if (payment != null)
                {
                    payment.CompletedAt = DateTime.UtcNow;
                    await _paymentRepository.UpdateAsync(payment);
                }
            }

            return result;
        }

        public async Task<bool> ProcessEscrowPaymentAsync(Guid campaignId, decimal amount)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
                throw new ArgumentException("Campaign not found");

            if (campaign.InfluencerId == null)
                throw new InvalidOperationException("Campaign does not have an assigned influencer");

            // Create escrow payment
            var createPaymentRequest = new CreatePaymentRequest
            {
                CampaignId = campaignId,
                RecipientId = Guid.Empty, // Platform escrow account
                Amount = amount,
                Currency = "NPR",
                Type = PaymentType.CampaignDeposit.ToString(),
                TransactionReference = null,
                PaymentMethod = "Wallet",
                Notes = $"Escrow payment for campaign: {campaign.Title}"
            };

            var payment = await CreatePaymentAsync(campaign.BrandId, createPaymentRequest);
            
            // Deduct from brand's wallet
            await _walletService.WithdrawFromWalletAsync(
                campaign.BrandId, 
                amount, 
                $"Escrow payment for campaign: {campaign.Title}", 
                payment.Id.ToString());

            return true;
        }

        public async Task<bool> ReleaseEscrowPaymentAsync(Guid paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
                throw new ArgumentException("Payment not found");

            if (payment.Type != PaymentType.CampaignDeposit)
                throw new InvalidOperationException("Payment is not an escrow payment");

            if (payment.Status != PaymentStatus.InEscrow)
                throw new InvalidOperationException("Payment is not in escrow status");

            var campaign = await _campaignRepository.GetByIdAsync(payment.CampaignId.Value);
            if (campaign == null)
                throw new ArgumentException("Campaign not found");

            if (campaign.InfluencerId == null)
                throw new InvalidOperationException("Campaign does not have an assigned influencer");

            // Update payment to transfer to influencer
            payment.RecipientId = campaign.InfluencerId.Value;
            payment.Status = PaymentStatus.Released;
            await _paymentRepository.UpdateAsync(payment);

            // Transfer to influencer's wallet (minus commission)
            await _walletService.DepositToWalletAsync(
                campaign.InfluencerId.Value,
                payment.NetAmount,
                $"Payment for campaign: {campaign.Title}",
                payment.Id.ToString());

            // Transfer commission to platform wallet
            // This would typically go to a platform admin wallet
            // For now, we'll just log it
            
            // Mark payment as completed
            payment.Status = PaymentStatus.Released;
            payment.CompletedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(payment);

            return true;
        }

        private PaymentDto MapToDto(Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                CampaignId = payment.CampaignId,
                SenderId = payment.SenderId,
                RecipientId = payment.RecipientId,
                Amount = payment.Amount,
                CommissionAmount = payment.CommissionAmount,
                NetAmount = payment.NetAmount,
                Currency = payment.Currency,
                Status = payment.Status.ToString(),
                Type = payment.Type.ToString(),
                TransactionReference = payment.TransactionReference,
                PaymentMethod = payment.PaymentMethod,
                Notes = payment.Notes,
                CreatedAt = payment.CreatedAt,
                CompletedAt = payment.CompletedAt
            };
        }

        private IEnumerable<PaymentDto> MapToDtoList(IEnumerable<Payment> payments)
        {
            var dtos = new List<PaymentDto>();
            foreach (var payment in payments)
            {
                dtos.Add(MapToDto(payment));
            }
            return dtos;
        }
    }
}