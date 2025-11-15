using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Shared.DTOs.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Campaign management endpoints for creating and managing marketing campaigns
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        /// <summary>
        /// Get a campaign by ID
        /// </summary>
        /// <param name="id">Campaign unique identifier</param>
        /// <returns>Campaign details</returns>
        /// <response code="200">Returns the campaign</response>
        /// <response code="404">Campaign not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CampaignDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CampaignDto>> GetCampaign(Guid id)
        {
            var campaign = await _campaignService.GetCampaignAsync(id);
            if (campaign == null)
                return NotFound();

            return Ok(campaign);
        }

        /// <summary>
        /// Get all campaigns for the authenticated brand user
        /// </summary>
        /// <returns>List of campaigns created by the brand</returns>
        /// <response code="200">Returns the list of campaigns</response>
        /// <response code="401">Unauthorized - Invalid or missing token</response>
        /// <response code="403">Forbidden - User is not a Brand</response>
        [Authorize(Roles = "Brand")]
        [HttpGet("brand")]
        [ProducesResponseType(typeof(IEnumerable<CampaignDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetBrandCampaigns()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var campaigns = await _campaignService.GetCampaignsByBrandAsync(userId);
            return Ok(campaigns);
        }

        [Authorize(Roles = "Influencer")]
        [HttpGet("influencer")]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetInfluencerCampaigns()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var campaigns = await _campaignService.GetCampaignsByInfluencerAsync(userId);
            return Ok(campaigns);
        }

        [Authorize(Roles = "Influencer")]
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetAvailableCampaigns()
        {
            var campaigns = await _campaignService.GetAvailableCampaignsAsync();
            return Ok(campaigns);
        }

        /// <summary>
        /// Create a new marketing campaign
        /// </summary>
        /// <param name="request">Campaign creation details</param>
        /// <returns>Created campaign</returns>
        /// <response code="201">Campaign created successfully</response>
        /// <response code="400">Invalid campaign data</response>
        /// <response code="401">Unauthorized - Invalid or missing token</response>
        /// <response code="403">Forbidden - User is not a Brand</response>
        [Authorize(Roles = "Brand")]
        [HttpPost]
        [ProducesResponseType(typeof(CampaignDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<CampaignDto>> CreateCampaign(CreateCampaignRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var campaign = await _campaignService.CreateCampaignAsync(userId, request);
                return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, campaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [Authorize(Roles = "Brand")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CampaignDto>> UpdateCampaign(Guid id, CreateCampaignRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Verify ownership
                var campaign = await _campaignService.GetCampaignAsync(id);
                if (campaign == null)
                    return NotFound();
                
                if (campaign.BrandId != userId)
                    return Forbid();

                var updatedCampaign = await _campaignService.UpdateCampaignAsync(id, request);
                return Ok(updatedCampaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Brand")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCampaign(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            // Verify ownership
            var campaign = await _campaignService.GetCampaignAsync(id);
            if (campaign == null)
                return NotFound();
            
            if (campaign.BrandId != userId)
                return Forbid();

            var result = await _campaignService.DeleteCampaignAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Influencer")]
        [HttpPost("{id}/apply")]
        public async Task<ActionResult> ApplyToCampaign(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            try
            {
                var result = await _campaignService.AssignInfluencerToCampaignAsync(id, userId);
                if (!result)
                    return NotFound();

                return Ok(new { message = "Successfully applied to campaign" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Brand")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateCampaignStatus(Guid id, [FromBody] UpdateStatusRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            // Verify ownership
            var campaign = await _campaignService.GetCampaignAsync(id);
            if (campaign == null)
                return NotFound();
            
            if (campaign.BrandId != userId)
                return Forbid();

            try
            {
                var result = await _campaignService.UpdateCampaignStatusAsync(id, request.Status);
                if (!result)
                    return NotFound();

                return Ok(new { message = "Campaign status updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Search and filter campaigns
        /// </summary>
        /// <param name="status">Campaign status filter</param>
        /// <param name="minBudget">Minimum budget filter</param>
        /// <param name="maxBudget">Maximum budget filter</param>
        /// <param name="startDate">Start date filter</param>
        /// <param name="endDate">End date filter</param>
        /// <param name="platform">Platform filter</param>
        /// <param name="niche">Niche filter</param>
        /// <returns>List of filtered campaigns</returns>
        /// <response code="200">Returns the list of filtered campaigns</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<CampaignDto>), 200)]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> SearchCampaigns(
            [FromQuery] string status = null,
            [FromQuery] decimal? minBudget = null,
            [FromQuery] decimal? maxBudget = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string platform = null,
            [FromQuery] string niche = null)
        {
            var campaigns = await _campaignService.SearchCampaignsAsync(status, minBudget, maxBudget, startDate, endDate, platform, niche);
            return Ok(campaigns);
        }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; }
    }
}