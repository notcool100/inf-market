using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;
using InfluencerMarketplace.Shared.DTOs.InfluencerProfile;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class InfluencerProfileService : IInfluencerProfileService
    {
        private readonly IInfluencerProfileRepository _influencerProfileRepository;
        private readonly IUserRepository _userRepository;

        public InfluencerProfileService(
            IInfluencerProfileRepository influencerProfileRepository,
            IUserRepository userRepository)
        {
            _influencerProfileRepository = influencerProfileRepository;
            _userRepository = userRepository;
        }

        public async Task<InfluencerProfileDto> GetInfluencerProfileAsync(Guid id)
        {
            var profile = await _influencerProfileRepository.GetByIdAsync(id);
            if (profile == null)
                return null;

            return MapToDto(profile);
        }

        public async Task<InfluencerProfileDto> GetInfluencerProfileByUserIdAsync(Guid userId)
        {
            var profile = await _influencerProfileRepository.GetByUserIdAsync(userId);
            if (profile == null)
                return null;

            return MapToDto(profile);
        }

        public async Task<IEnumerable<InfluencerProfileDto>> SearchInfluencersAsync(
            string nicheFocus, string location, int? minFollowers, decimal? maxRate)
        {
            var profiles = await _influencerProfileRepository.SearchInfluencersAsync(
                nicheFocus, location, minFollowers, maxRate);
            
            return MapToDtoList(profiles);
        }

        public async Task<InfluencerProfileDto> CreateInfluencerProfileAsync(
            Guid userId, CreateInfluencerProfileRequest request)
        {
            // Validate user exists and has Influencer role
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var roles = await _userRepository.GetUserRolesAsync(userId);
            if (!roles.Contains("Influencer"))
                throw new UnauthorizedAccessException("User is not an influencer");

            // Check if profile already exists
            var existingProfile = await _influencerProfileRepository.GetByUserIdAsync(userId);
            if (existingProfile != null)
                throw new InvalidOperationException("Influencer profile already exists for this user");

            var profile = new InfluencerProfile
            {
                UserId = userId,
                Bio = request.Bio,
                NicheFocus = request.NicheFocus,
                FollowersCount = request.FollowersCount,
                InstagramHandle = request.InstagramHandle,
                TikTokHandle = request.TikTokHandle,
                YouTubeChannel = request.YouTubeChannel,
                FacebookPage = request.FacebookPage,
                LinkedInProfile = request.LinkedInProfile,
                WebsiteUrl = request.WebsiteUrl,
                MinCampaignRate = request.MinCampaignRate,
                ContentTypes = JsonSerializer.Serialize(request.ContentTypes ?? new List<string>()),
                Demographics = JsonSerializer.Serialize(request.Demographics ?? new Dictionary<string, object>()),
                Location = request.Location,
                IsVerified = false, // New profiles start unverified
                AverageRating = 0,
                CompletedCampaigns = 0
            };

            var profileId = await _influencerProfileRepository.CreateAsync(profile);
            profile.Id = profileId;

            return MapToDto(profile);
        }

        public async Task<InfluencerProfileDto> UpdateInfluencerProfileAsync(
            Guid id, CreateInfluencerProfileRequest request)
        {
            var existingProfile = await _influencerProfileRepository.GetByIdAsync(id);
            if (existingProfile == null)
                throw new ArgumentException("Influencer profile not found");

            existingProfile.Bio = request.Bio;
            existingProfile.NicheFocus = request.NicheFocus;
            existingProfile.FollowersCount = request.FollowersCount;
            existingProfile.InstagramHandle = request.InstagramHandle;
            existingProfile.TikTokHandle = request.TikTokHandle;
            existingProfile.YouTubeChannel = request.YouTubeChannel;
            existingProfile.FacebookPage = request.FacebookPage;
            existingProfile.LinkedInProfile = request.LinkedInProfile;
            existingProfile.WebsiteUrl = request.WebsiteUrl;
            existingProfile.MinCampaignRate = request.MinCampaignRate;
            existingProfile.ContentTypes = JsonSerializer.Serialize(request.ContentTypes ?? new List<string>());
            existingProfile.Demographics = JsonSerializer.Serialize(request.Demographics ?? new Dictionary<string, object>());
            existingProfile.Location = request.Location;

            await _influencerProfileRepository.UpdateAsync(existingProfile);

            return MapToDto(existingProfile);
        }

        public async Task<bool> DeleteInfluencerProfileAsync(Guid id)
        {
            var profile = await _influencerProfileRepository.GetByIdAsync(id);
            if (profile == null)
                return false;

            return await _influencerProfileRepository.DeleteAsync(id);
        }

        private InfluencerProfileDto MapToDto(InfluencerProfile profile)
        {
            return new InfluencerProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Bio = profile.Bio,
                NicheFocus = profile.NicheFocus,
                FollowersCount = profile.FollowersCount,
                InstagramHandle = profile.InstagramHandle,
                TikTokHandle = profile.TikTokHandle,
                YouTubeChannel = profile.YouTubeChannel,
                FacebookPage = profile.FacebookPage,
                LinkedInProfile = profile.LinkedInProfile,
                WebsiteUrl = profile.WebsiteUrl,
                MinCampaignRate = profile.MinCampaignRate,
                ContentTypes = string.IsNullOrEmpty(profile.ContentTypes) 
                    ? new List<string>() 
                    : JsonSerializer.Deserialize<List<string>>(profile.ContentTypes) ?? new List<string>(),
                Demographics = string.IsNullOrEmpty(profile.Demographics) 
                    ? new Dictionary<string, object>() 
                    : JsonSerializer.Deserialize<Dictionary<string, object>>(profile.Demographics) ?? new Dictionary<string, object>(),
                Location = profile.Location,
                IsVerified = profile.IsVerified,
                AverageRating = profile.AverageRating,
                CompletedCampaigns = profile.CompletedCampaigns,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt
            };
        }

        private IEnumerable<InfluencerProfileDto> MapToDtoList(IEnumerable<InfluencerProfile> profiles)
        {
            var dtos = new List<InfluencerProfileDto>();
            foreach (var profile in profiles)
            {
                dtos.Add(MapToDto(profile));
            }
            return dtos;
        }
    }
}