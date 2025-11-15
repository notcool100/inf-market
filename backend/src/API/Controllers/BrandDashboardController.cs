using System;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Brand dashboard statistics and analytics endpoints
    /// </summary>
    [ApiController]
    [Route("api/brand/dashboard")]
    [Authorize(Roles = "Brand")]
    [Produces("application/json")]
    public class BrandDashboardController : ControllerBase
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IPaymentRepository _paymentRepository;

        public BrandDashboardController(
            ICampaignRepository campaignRepository,
            IPaymentRepository paymentRepository)
        {
            _campaignRepository = campaignRepository;
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// Get brand dashboard statistics
        /// </summary>
        /// <returns>Brand dashboard statistics including campaigns, spending, and payments</returns>
        /// <response code="200">Returns the dashboard statistics</response>
        /// <response code="401">Unauthorized - Invalid or missing token</response>
        /// <response code="403">Forbidden - User is not a Brand</response>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(BrandDashboardStats), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<BrandDashboardStats>> GetStats()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Get all campaigns for this brand
            var campaigns = await _campaignRepository.GetCampaignsByBrandIdAsync(userId);

            // Calculate statistics
            var totalCampaigns = 0;
            var activeCampaigns = 0;
            decimal totalSpent = 0;
            decimal pendingPayments = 0;

            foreach (var campaign in campaigns)
            {
                totalCampaigns++;
                
                if (campaign.Status == Core.Models.CampaignStatus.Open || 
                    campaign.Status == Core.Models.CampaignStatus.InProgress ||
                    campaign.Status == Core.Models.CampaignStatus.PendingReview)
                {
                    activeCampaigns++;
                }

                // Get payments for this campaign
                var payments = await _paymentRepository.FindAsync(
                    "SELECT * FROM Payments WHERE CampaignId = @CampaignId AND SenderId = @SenderId",
                    new { CampaignId = campaign.Id, SenderId = userId });

                foreach (var payment in payments)
                {
                    if (payment.Status == Core.Models.PaymentStatus.Released)
                    {
                        totalSpent += payment.Amount;
                    }
                    else if (payment.Status == Core.Models.PaymentStatus.Pending || 
                             payment.Status == Core.Models.PaymentStatus.InEscrow)
                    {
                        pendingPayments += payment.Amount;
                    }
                }
            }

            var stats = new BrandDashboardStats
            {
                TotalCampaigns = totalCampaigns,
                ActiveCampaigns = activeCampaigns,
                TotalSpent = totalSpent,
                PendingPayments = pendingPayments
            };

            return Ok(stats);
        }
    }

    /// <summary>
    /// Brand dashboard statistics response model
    /// </summary>
    public class BrandDashboardStats
    {
        /// <summary>
        /// Total number of campaigns created by the brand
        /// </summary>
        public int TotalCampaigns { get; set; }

        /// <summary>
        /// Number of active campaigns (Open, InProgress, PendingReview)
        /// </summary>
        public int ActiveCampaigns { get; set; }

        /// <summary>
        /// Total amount spent on completed payments
        /// </summary>
        public decimal TotalSpent { get; set; }

        /// <summary>
        /// Total amount in pending payments (Pending or InEscrow)
        /// </summary>
        public decimal PendingPayments { get; set; }
    }
}

