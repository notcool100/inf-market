using System;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Shared.DTOs.CampaignDeliverable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Campaign deliverables management endpoints
    /// </summary>
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class CampaignDeliverableController : ControllerBase
    {
        private readonly ICampaignDeliverableService _deliverableService;
        private readonly ICampaignService _campaignService;

        public CampaignDeliverableController(
            ICampaignDeliverableService deliverableService,
            ICampaignService campaignService)
        {
            _deliverableService = deliverableService;
            _campaignService = campaignService;
        }

        /// <summary>
        /// Get all deliverables for a campaign
        /// </summary>
        /// <param name="campaignId">Campaign unique identifier</param>
        /// <returns>List of deliverables</returns>
        /// <response code="200">Returns the list of deliverables</response>
        [HttpGet("campaigns/{campaignId}/deliverables")]
        [ProducesResponseType(typeof(System.Collections.Generic.IEnumerable<CampaignDeliverableDto>), 200)]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<CampaignDeliverableDto>>> GetCampaignDeliverables(Guid campaignId)
        {
            var deliverables = await _deliverableService.GetDeliverablesByCampaignIdAsync(campaignId);
            return Ok(deliverables);
        }

        /// <summary>
        /// Get a specific deliverable by ID
        /// </summary>
        /// <param name="id">Deliverable unique identifier</param>
        /// <returns>Deliverable details</returns>
        /// <response code="200">Returns the deliverable</response>
        /// <response code="404">Deliverable not found</response>
        [HttpGet("deliverables/{id}")]
        [ProducesResponseType(typeof(CampaignDeliverableDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CampaignDeliverableDto>> GetDeliverable(Guid id)
        {
            var deliverable = await _deliverableService.GetDeliverableAsync(id);
            if (deliverable == null)
                return NotFound();

            return Ok(deliverable);
        }

        /// <summary>
        /// Create a new deliverable for a campaign (Brand only)
        /// </summary>
        /// <param name="campaignId">Campaign unique identifier</param>
        /// <param name="request">Deliverable creation details</param>
        /// <returns>Created deliverable</returns>
        /// <response code="201">Deliverable created successfully</response>
        /// <response code="400">Invalid deliverable data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not a Brand</response>
        [Authorize(Roles = "Brand")]
        [HttpPost("campaigns/{campaignId}/deliverables")]
        [ProducesResponseType(typeof(CampaignDeliverableDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<CampaignDeliverableDto>> CreateDeliverable(Guid campaignId, CreateDeliverableRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Verify campaign ownership
                var campaign = await _campaignService.GetCampaignAsync(campaignId);
                if (campaign == null)
                    return NotFound();
                
                if (campaign.BrandId != userId)
                    return Forbid();

                var deliverable = await _deliverableService.CreateDeliverableAsync(campaignId, request);
                return CreatedAtAction(nameof(GetDeliverable), new { id = deliverable.Id }, deliverable);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a deliverable (Brand only)
        /// </summary>
        /// <param name="id">Deliverable unique identifier</param>
        /// <param name="request">Updated deliverable details</param>
        /// <returns>Updated deliverable</returns>
        /// <response code="200">Deliverable updated successfully</response>
        /// <response code="400">Invalid deliverable data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not a Brand</response>
        /// <response code="404">Deliverable not found</response>
        [Authorize(Roles = "Brand")]
        [HttpPut("deliverables/{id}")]
        [ProducesResponseType(typeof(CampaignDeliverableDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CampaignDeliverableDto>> UpdateDeliverable(Guid id, CreateDeliverableRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Verify campaign ownership
                var deliverable = await _deliverableService.GetDeliverableAsync(id);
                if (deliverable == null)
                    return NotFound();
                
                var campaign = await _campaignService.GetCampaignAsync(deliverable.CampaignId);
                if (campaign == null || campaign.BrandId != userId)
                    return Forbid();

                var updatedDeliverable = await _deliverableService.UpdateDeliverableAsync(id, request);
                return Ok(updatedDeliverable);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Submit deliverable proof (Influencer only)
        /// </summary>
        /// <param name="id">Deliverable unique identifier</param>
        /// <param name="request">Submission details with proof URLs</param>
        /// <returns>Success message</returns>
        /// <response code="200">Deliverable submitted successfully</response>
        /// <response code="400">Invalid submission data or deliverable not in pending status</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not an Influencer</response>
        /// <response code="404">Deliverable not found</response>
        [Authorize(Roles = "Influencer")]
        [HttpPost("deliverables/{id}/submit")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> SubmitDeliverable(Guid id, SubmitDeliverableRequest request)
        {
            try
            {
                var result = await _deliverableService.SubmitDeliverableAsync(id, request);
                if (!result)
                    return NotFound();

                return Ok(new { message = "Deliverable submitted successfully" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Approve a deliverable (Brand only)
        /// </summary>
        /// <param name="id">Deliverable unique identifier</param>
        /// <param name="request">Review details with optional feedback</param>
        /// <returns>Success message</returns>
        /// <response code="200">Deliverable approved successfully</response>
        /// <response code="400">Invalid review data or deliverable not in submitted status</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not a Brand</response>
        /// <response code="404">Deliverable not found</response>
        [Authorize(Roles = "Brand")]
        [HttpPost("deliverables/{id}/approve")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> ApproveDeliverable(Guid id, ReviewDeliverableRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Verify campaign ownership
                var deliverable = await _deliverableService.GetDeliverableAsync(id);
                if (deliverable == null)
                    return NotFound();
                
                var campaign = await _campaignService.GetCampaignAsync(deliverable.CampaignId);
                if (campaign == null || campaign.BrandId != userId)
                    return Forbid();

                var result = await _deliverableService.ApproveDeliverableAsync(id, request);
                if (!result)
                    return NotFound();

                return Ok(new { message = "Deliverable approved successfully" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Reject a deliverable (Brand only)
        /// </summary>
        /// <param name="id">Deliverable unique identifier</param>
        /// <param name="request">Review details with feedback notes</param>
        /// <returns>Success message</returns>
        /// <response code="200">Deliverable rejected successfully</response>
        /// <response code="400">Invalid review data or deliverable not in submitted status</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - User is not a Brand</response>
        /// <response code="404">Deliverable not found</response>
        [Authorize(Roles = "Brand")]
        [HttpPost("deliverables/{id}/reject")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> RejectDeliverable(Guid id, ReviewDeliverableRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Verify campaign ownership
                var deliverable = await _deliverableService.GetDeliverableAsync(id);
                if (deliverable == null)
                    return NotFound();
                
                var campaign = await _campaignService.GetCampaignAsync(deliverable.CampaignId);
                if (campaign == null || campaign.BrandId != userId)
                    return Forbid();

                var result = await _deliverableService.RejectDeliverableAsync(id, request);
                if (!result)
                    return NotFound();

                return Ok(new { message = "Deliverable rejected successfully" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

