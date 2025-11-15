using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Influencer dashboard statistics and analytics endpoints
    /// </summary>
    [ApiController]
    [Route("api/influencer/dashboard")]
    [Authorize(Roles = "Influencer")]
    [Produces("application/json")]
    public class InfluencerDashboardController : ControllerBase
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInfluencerProfileRepository _influencerProfileRepository;

        public InfluencerDashboardController(
            ICampaignRepository campaignRepository,
            IPaymentRepository paymentRepository,
            IInfluencerProfileRepository influencerProfileRepository)
        {
            _campaignRepository = campaignRepository;
            _paymentRepository = paymentRepository;
            _influencerProfileRepository = influencerProfileRepository;
        }

        /// <summary>
        /// Get influencer dashboard statistics
        /// </summary>
        /// <returns>Influencer dashboard statistics including campaigns, earnings, and ratings</returns>
        /// <response code="200">Returns the dashboard statistics</response>
        /// <response code="401">Unauthorized - Invalid or missing token</response>
        /// <response code="403">Forbidden - User is not an Influencer</response>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(InfluencerDashboardStats), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<InfluencerDashboardStats>> GetStats()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Get influencer profile
            var profile = await _influencerProfileRepository.FindAsync(
                "SELECT * FROM InfluencerProfiles WHERE UserId = @UserId",
                new { UserId = userId });

            var influencerProfile = profile.FirstOrDefault();
            if (influencerProfile == null)
            {
                return NotFound(new { message = "Influencer profile not found" });
            }

            // Get all campaigns for this influencer
            var campaigns = await _campaignRepository.GetCampaignsByInfluencerIdAsync(influencerProfile.Id);

            // Calculate statistics
            var totalCampaigns = 0;
            var activeCampaigns = 0;
            decimal totalEarnings = 0;
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
                    "SELECT * FROM Payments WHERE CampaignId = @CampaignId AND RecipientId = @RecipientId",
                    new { CampaignId = campaign.Id, RecipientId = userId });

                foreach (var payment in payments)
                {
                    if (payment.Status == Core.Models.PaymentStatus.Released)
                    {
                        totalEarnings += payment.NetAmount; // Net amount after commission
                    }
                    else if (payment.Status == Core.Models.PaymentStatus.Pending || 
                             payment.Status == Core.Models.PaymentStatus.InEscrow)
                    {
                        pendingPayments += payment.NetAmount;
                    }
                }
            }

            var stats = new InfluencerDashboardStats
            {
                TotalCampaigns = totalCampaigns,
                ActiveCampaigns = activeCampaigns,
                TotalEarnings = totalEarnings,
                PendingPayments = pendingPayments,
                AverageRating = influencerProfile.AverageRating,
                CompletedCampaigns = influencerProfile.CompletedCampaigns
            };

            return Ok(stats);
        }
    }

    /// <summary>
    /// Influencer dashboard statistics response model
    /// </summary>
    public class InfluencerDashboardStats
    {
        /// <summary>
        /// Total number of campaigns assigned to the influencer
        /// </summary>
        public int TotalCampaigns { get; set; }

        /// <summary>
        /// Number of active campaigns (Open, InProgress, PendingReview)
        /// </summary>
        public int ActiveCampaigns { get; set; }

        /// <summary>
        /// Total earnings from completed payments (net amount after commission)
        /// </summary>
        public decimal TotalEarnings { get; set; }

        /// <summary>
        /// Total amount in pending payments (Pending or InEscrow)
        /// </summary>
        public decimal PendingPayments { get; set; }

        /// <summary>
        /// Average rating from reviews (1-5 stars)
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Number of completed campaigns
        /// </summary>
        public int CompletedCampaigns { get; set; }
    }
}

