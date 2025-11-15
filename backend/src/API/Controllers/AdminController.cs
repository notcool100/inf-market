using System;
using System.Collections.Generic;
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
    /// Admin panel endpoints for platform management
    /// </summary>
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInfluencerProfileRepository _influencerProfileRepository;

        public AdminController(
            IUserRepository userRepository,
            ICampaignRepository campaignRepository,
            IPaymentRepository paymentRepository,
            IInfluencerProfileRepository influencerProfileRepository)
        {
            _userRepository = userRepository;
            _campaignRepository = campaignRepository;
            _paymentRepository = paymentRepository;
            _influencerProfileRepository = influencerProfileRepository;
        }

        /// <summary>
        /// Get all users in the platform
        /// </summary>
        /// <returns>List of all users</returns>
        /// <response code="200">Returns the list of users</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not an Admin</response>
        [HttpGet("users")]
        [ProducesResponseType(typeof(System.Collections.Generic.IEnumerable<UserSummaryDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<UserSummaryDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            var userList = users.Select(u => new UserSummaryDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });
            return Ok(userList);
        }

        /// <summary>
        /// Get user details by ID
        /// </summary>
        /// <param name="id">User unique identifier</param>
        /// <returns>User details</returns>
        /// <response code="200">Returns the user details</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not an Admin</response>
        /// <response code="404">User not found</response>
        [HttpGet("users/{id}")]
        [ProducesResponseType(typeof(UserDetailsDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDetailsDto>> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var userDto = new UserDetailsDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Activate or deactivate a user
        /// </summary>
        /// <param name="id">User unique identifier</param>
        /// <param name="request">Status update request</param>
        /// <returns>Success message</returns>
        /// <response code="200">User status updated successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not an Admin</response>
        /// <response code="404">User not found</response>
        [HttpPut("users/{id}/status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateUserStatus(Guid id, [FromBody] UpdateUserStatusRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            user.IsActive = request.IsActive;
            await _userRepository.UpdateAsync(user);

            return Ok(new { message = $"User {(request.IsActive ? "activated" : "deactivated")} successfully" });
        }

        /// <summary>
        /// Get all campaigns in the platform
        /// </summary>
        /// <returns>List of all campaigns</returns>
        /// <response code="200">Returns the list of campaigns</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not an Admin</response>
        [HttpGet("campaigns")]
        [ProducesResponseType(typeof(System.Collections.Generic.IEnumerable<CampaignSummaryDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<CampaignSummaryDto>>> GetCampaigns()
        {
            var campaigns = await _campaignRepository.GetAllAsync();
            var campaignList = campaigns.Select(c => new CampaignSummaryDto
            {
                Id = c.Id,
                Title = c.Title,
                BrandId = c.BrandId,
                InfluencerId = c.InfluencerId,
                Budget = c.Budget,
                Status = c.Status.ToString(),
                CreatedAt = c.CreatedAt
            });
            return Ok(campaignList);
        }

        /// <summary>
        /// Get all payments in the platform
        /// </summary>
        /// <returns>List of all payments</returns>
        /// <response code="200">Returns the list of payments</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not an Admin</response>
        [HttpGet("payments")]
        [ProducesResponseType(typeof(System.Collections.Generic.IEnumerable<PaymentSummaryDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<PaymentSummaryDto>>> GetPayments()
        {
            var payments = await _paymentRepository.GetAllAsync();
            var paymentList = payments.Select(p => new PaymentSummaryDto
            {
                Id = p.Id,
                CampaignId = p.CampaignId,
                SenderId = p.SenderId,
                RecipientId = p.RecipientId,
                Amount = p.Amount,
                Status = p.Status.ToString(),
                Type = p.Type.ToString(),
                CreatedAt = p.CreatedAt
            });
            return Ok(paymentList);
        }

        /// <summary>
        /// Get platform statistics
        /// </summary>
        /// <returns>Platform statistics</returns>
        /// <response code="200">Returns platform statistics</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not an Admin</response>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(PlatformStatsDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<PlatformStatsDto>> GetStats()
        {
            var users = await _userRepository.GetAllAsync();
            var campaigns = await _campaignRepository.GetAllAsync();
            var payments = await _paymentRepository.GetAllAsync();
            var influencerProfiles = await _influencerProfileRepository.GetAllAsync();

            var userList = users.ToList();
            var campaignList = campaigns.ToList();
            var paymentList = payments.ToList();
            var influencerList = influencerProfiles.ToList();

            var stats = new PlatformStatsDto
            {
                TotalUsers = userList.Count,
                ActiveUsers = userList.Count(u => u.IsActive),
                TotalCampaigns = campaignList.Count,
                ActiveCampaigns = campaignList.Count(c => 
                    c.Status == Core.Models.CampaignStatus.Open || 
                    c.Status == Core.Models.CampaignStatus.InProgress),
                TotalPayments = paymentList.Count,
                TotalRevenue = paymentList.Where(p => p.Status == Core.Models.PaymentStatus.Released)
                    .Sum(p => p.CommissionAmount),
                TotalInfluencers = influencerList.Count,
                VerifiedInfluencers = influencerList.Count(i => i.IsVerified)
            };

            return Ok(stats);
        }

        /// <summary>
        /// Verify an influencer profile
        /// </summary>
        /// <param name="id">Influencer profile unique identifier</param>
        /// <returns>Success message</returns>
        /// <response code="200">Influencer verified successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not an Admin</response>
        /// <response code="404">Influencer profile not found</response>
        [HttpPost("influencers/verify/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> VerifyInfluencer(Guid id)
        {
            var influencerProfiles = await _influencerProfileRepository.FindAsync(
                "SELECT * FROM InfluencerProfiles WHERE Id = @Id",
                new { Id = id });

            var profile = influencerProfiles.FirstOrDefault();
            if (profile == null)
                return NotFound();

            profile.IsVerified = true;
            await _influencerProfileRepository.UpdateAsync(profile);

            return Ok(new { message = "Influencer verified successfully" });
        }
    }

    /// <summary>
    /// User summary DTO
    /// </summary>
    public class UserSummaryDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// User details DTO
    /// </summary>
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Update user status request
    /// </summary>
    public class UpdateUserStatusRequest
    {
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Campaign summary DTO
    /// </summary>
    public class CampaignSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid BrandId { get; set; }
        public Guid? InfluencerId { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Payment summary DTO
    /// </summary>
    public class PaymentSummaryDto
    {
        public Guid Id { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecipientId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Platform statistics DTO
    /// </summary>
    public class PlatformStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalCampaigns { get; set; }
        public int ActiveCampaigns { get; set; }
        public int TotalPayments { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalInfluencers { get; set; }
        public int VerifiedInfluencers { get; set; }
    }
}

