using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Shared.DTOs.InfluencerProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfluencerProfileController : ControllerBase
    {
        private readonly IInfluencerProfileService _influencerProfileService;

        public InfluencerProfileController(IInfluencerProfileService influencerProfileService)
        {
            _influencerProfileService = influencerProfileService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InfluencerProfileDto>> GetInfluencerProfile(Guid id)
        {
            var profile = await _influencerProfileService.GetInfluencerProfileAsync(id);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<InfluencerProfileDto>> GetInfluencerProfileByUserId(Guid userId)
        {
            var profile = await _influencerProfileService.GetInfluencerProfileByUserIdAsync(userId);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [Authorize(Roles = "Influencer")]
        [HttpGet("me")]
        public async Task<ActionResult<InfluencerProfileDto>> GetMyProfile()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var profile = await _influencerProfileService.GetInfluencerProfileByUserIdAsync(userId);
            
            if (profile == null)
                return NotFound(new { message = "Profile not found. Please create your profile." });

            return Ok(profile);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<InfluencerProfileDto>>> SearchInfluencers(
            [FromQuery] string nicheFocus = null,
            [FromQuery] string location = null,
            [FromQuery] int? minFollowers = null,
            [FromQuery] decimal? maxRate = null)
        {
            var profiles = await _influencerProfileService.SearchInfluencersAsync(
                nicheFocus, location, minFollowers, maxRate);
            
            return Ok(profiles);
        }

        [Authorize(Roles = "Influencer")]
        [HttpPost]
        public async Task<ActionResult<InfluencerProfileDto>> CreateInfluencerProfile(CreateInfluencerProfileRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var profile = await _influencerProfileService.CreateInfluencerProfileAsync(userId, request);
                return CreatedAtAction(nameof(GetInfluencerProfile), new { id = profile.Id }, profile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = "Influencer")]
        [HttpPut("{id}")]
        public async Task<ActionResult<InfluencerProfileDto>> UpdateInfluencerProfile(Guid id, CreateInfluencerProfileRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Verify ownership
                var profile = await _influencerProfileService.GetInfluencerProfileAsync(id);
                if (profile == null)
                    return NotFound();
                
                if (profile.UserId != userId)
                    return Forbid();

                var updatedProfile = await _influencerProfileService.UpdateInfluencerProfileAsync(id, request);
                return Ok(updatedProfile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Influencer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInfluencerProfile(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            // Verify ownership
            var profile = await _influencerProfileService.GetInfluencerProfileAsync(id);
            if (profile == null)
                return NotFound();
            
            if (profile.UserId != userId)
                return Forbid();

            var result = await _influencerProfileService.DeleteInfluencerProfileAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}