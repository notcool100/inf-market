using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;
using InfluencerMarketplace.Shared.DTOs.Campaign;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IUserRepository _userRepository;

        public CampaignService(ICampaignRepository campaignRepository, IUserRepository userRepository)
        {
            _campaignRepository = campaignRepository;
            _userRepository = userRepository;
        }

        public async Task<CampaignDto> GetCampaignAsync(Guid id)
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign == null)
                return null;

            return MapToDto(campaign);
        }

        public async Task<IEnumerable<CampaignDto>> GetCampaignsByBrandAsync(Guid brandId)
        {
            var campaigns = await _campaignRepository.GetCampaignsByBrandIdAsync(brandId);
            return MapToDtoList(campaigns);
        }

        public async Task<IEnumerable<CampaignDto>> GetCampaignsByInfluencerAsync(Guid influencerId)
        {
            var campaigns = await _campaignRepository.GetCampaignsByInfluencerIdAsync(influencerId);
            return MapToDtoList(campaigns);
        }

        public async Task<IEnumerable<CampaignDto>> GetAvailableCampaignsAsync()
        {
            var campaigns = await _campaignRepository.GetAvailableCampaignsAsync();
            return MapToDtoList(campaigns);
        }

        public async Task<CampaignDto> CreateCampaignAsync(Guid brandId, CreateCampaignRequest request)
        {
            // Validate brand exists
            var brand = await _userRepository.GetByIdAsync(brandId);
            if (brand == null)
                throw new ArgumentException("Brand not found");

            // Check if user has Brand role
            var roles = await _userRepository.GetUserRolesAsync(brandId);
            if (!roles.Contains("Brand"))
                throw new UnauthorizedAccessException("User is not a brand");

            var campaign = new Campaign
            {
                Title = request.Title,
                Description = request.Description,
                BrandId = brandId,
                Budget = request.Budget,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Requirements = request.Requirements,
                Deliverables = JsonSerializer.Serialize(request.Deliverables ?? new List<string>()),
                TargetAudience = JsonSerializer.Serialize(request.TargetAudience ?? new Dictionary<string, object>()),
                TargetPlatforms = JsonSerializer.Serialize(request.TargetPlatforms ?? new List<string>()),
                Status = CampaignStatus.Draft
            };

            var campaignId = await _campaignRepository.CreateAsync(campaign);
            campaign.Id = campaignId;

            return MapToDto(campaign);
        }

        public async Task<CampaignDto> UpdateCampaignAsync(Guid id, CreateCampaignRequest request)
        {
            var existingCampaign = await _campaignRepository.GetByIdAsync(id);
            if (existingCampaign == null)
                throw new ArgumentException("Campaign not found");

            existingCampaign.Title = request.Title;
            existingCampaign.Description = request.Description;
            existingCampaign.Budget = request.Budget;
            existingCampaign.StartDate = request.StartDate;
            existingCampaign.EndDate = request.EndDate;
            existingCampaign.Requirements = request.Requirements;
            existingCampaign.Deliverables = JsonSerializer.Serialize(request.Deliverables ?? new List<string>());
            existingCampaign.TargetAudience = JsonSerializer.Serialize(request.TargetAudience ?? new Dictionary<string, object>());
            existingCampaign.TargetPlatforms = JsonSerializer.Serialize(request.TargetPlatforms ?? new List<string>());

            await _campaignRepository.UpdateAsync(existingCampaign);

            return MapToDto(existingCampaign);
        }

        public async Task<bool> DeleteCampaignAsync(Guid id)
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign == null)
                return false;

            return await _campaignRepository.DeleteAsync(id);
        }

        public async Task<bool> AssignInfluencerToCampaignAsync(Guid campaignId, Guid influencerId)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
                return false;

            // Validate influencer exists and has Influencer role
            var influencer = await _userRepository.GetByIdAsync(influencerId);
            if (influencer == null)
                return false;

            var roles = await _userRepository.GetUserRolesAsync(influencerId);
            if (!roles.Contains("Influencer"))
                return false;

            return await _campaignRepository.AssignInfluencerAsync(campaignId, influencerId);
        }

        public async Task<bool> UpdateCampaignStatusAsync(Guid campaignId, string status)
        {
            if (!Enum.TryParse<CampaignStatus>(status, out var campaignStatus))
                throw new ArgumentException("Invalid campaign status");

            return await _campaignRepository.UpdateCampaignStatusAsync(campaignId, campaignStatus);
        }

        public async Task<IEnumerable<CampaignDto>> SearchCampaignsAsync(string status = null, decimal? minBudget = null, decimal? maxBudget = null, DateTime? startDate = null, DateTime? endDate = null, string platform = null, string niche = null)
        {
            var campaigns = await _campaignRepository.SearchCampaignsAsync(status, minBudget, maxBudget, startDate, endDate, platform, niche);
            return MapToDtoList(campaigns);
        }

        private CampaignDto MapToDto(Campaign campaign)
        {
            return new CampaignDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                BrandId = campaign.BrandId,
                InfluencerId = campaign.InfluencerId,
                Budget = campaign.Budget,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                Requirements = campaign.Requirements,
                Deliverables = string.IsNullOrEmpty(campaign.Deliverables) 
                    ? new List<string>() 
                    : JsonSerializer.Deserialize<List<string>>(campaign.Deliverables) ?? new List<string>(),
                TargetAudience = string.IsNullOrEmpty(campaign.TargetAudience) 
                    ? new Dictionary<string, object>() 
                    : JsonSerializer.Deserialize<Dictionary<string, object>>(campaign.TargetAudience) ?? new Dictionary<string, object>(),
                TargetPlatforms = string.IsNullOrEmpty(campaign.TargetPlatforms) 
                    ? new List<string>() 
                    : JsonSerializer.Deserialize<List<string>>(campaign.TargetPlatforms) ?? new List<string>(),
                Status = campaign.Status.ToString(),
                CreatedAt = campaign.CreatedAt,
                UpdatedAt = campaign.UpdatedAt
            };
        }

        private IEnumerable<CampaignDto> MapToDtoList(IEnumerable<Campaign> campaigns)
        {
            var dtos = new List<CampaignDto>();
            foreach (var campaign in campaigns)
            {
                dtos.Add(MapToDto(campaign));
            }
            return dtos;
        }
    }
}