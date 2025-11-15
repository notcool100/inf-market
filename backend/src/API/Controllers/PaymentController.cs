using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Shared.DTOs.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Payment management endpoints for processing payments and escrow transactions
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ICampaignService _campaignService;

        public PaymentController(IPaymentService paymentService, ICampaignService campaignService)
        {
            _paymentService = paymentService;
            _campaignService = campaignService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPayment(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var payment = await _paymentService.GetPaymentAsync(id);
            
            if (payment == null)
                return NotFound();

            // Only allow sender, recipient, or admin to view payment details
            if (payment.SenderId != userId && payment.RecipientId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return Ok(payment);
        }

        [HttpGet("campaign/{campaignId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByCampaign(Guid campaignId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            // Verify campaign ownership or participation
            var campaign = await _campaignService.GetCampaignAsync(campaignId);
            if (campaign == null)
                return NotFound();
            
            if (campaign.BrandId != userId && campaign.InfluencerId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var payments = await _paymentService.GetPaymentsByCampaignAsync(campaignId);
            return Ok(payments);
        }

        [HttpGet("sent")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetSentPayments()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var payments = await _paymentService.GetPaymentsBySenderAsync(userId);
            return Ok(payments);
        }

        [HttpGet("received")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetReceivedPayments()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var payments = await _paymentService.GetPaymentsByRecipientAsync(userId);
            return Ok(payments);
        }

        [Authorize(Roles = "Brand")]
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment(CreatePaymentRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // If payment is for a campaign, verify campaign ownership
                if (request.CampaignId.HasValue)
                {
                    var campaign = await _campaignService.GetCampaignAsync(request.CampaignId.Value);
                    if (campaign == null)
                        return NotFound("Campaign not found");
                    
                    if (campaign.BrandId != userId)
                        return Forbid();
                }

                var payment = await _paymentService.CreatePaymentAsync(userId, request);
                return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Brand,Admin")]
        [HttpPost("escrow/{campaignId}")]
        public async Task<ActionResult> ProcessEscrowPayment(Guid campaignId, [FromBody] EscrowPaymentRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Verify campaign ownership
                var campaign = await _campaignService.GetCampaignAsync(campaignId);
                if (campaign == null)
                    return NotFound("Campaign not found");
                
                if (campaign.BrandId != userId && !User.IsInRole("Admin"))
                    return Forbid();

                var result = await _paymentService.ProcessEscrowPaymentAsync(campaignId, request.Amount);
                return Ok(new { message = "Escrow payment processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Brand,Admin")]
        [HttpPost("release/{paymentId}")]
        public async Task<ActionResult> ReleaseEscrowPayment(Guid paymentId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Verify payment ownership
                var payment = await _paymentService.GetPaymentAsync(paymentId);
                if (payment == null)
                    return NotFound("Payment not found");
                
                if (payment.SenderId != userId && !User.IsInRole("Admin"))
                    return Forbid();

                var result = await _paymentService.ReleaseEscrowPaymentAsync(paymentId);
                return Ok(new { message = "Escrow payment released successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdatePaymentStatus(Guid id, [FromBody] UpdatePaymentStatusRequest request)
        {
            try
            {
                var result = await _paymentService.UpdatePaymentStatusAsync(id, request.Status);
                if (!result)
                    return NotFound();

                return Ok(new { message = "Payment status updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class EscrowPaymentRequest
    {
        public decimal Amount { get; set; }
    }

    public class UpdatePaymentStatusRequest
    {
        public string Status { get; set; }
    }
}