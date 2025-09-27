using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Shared.DTOs.InfluencerProfile;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface IInfluencerProfileService
    {
        Task<InfluencerProfileDto> GetInfluencerProfileAsync(Guid id);
        Task<InfluencerProfileDto> GetInfluencerProfileByUserIdAsync(Guid userId);
        Task<IEnumerable<InfluencerProfileDto>> SearchInfluencersAsync(string nicheFocus, string location, int? minFollowers, decimal? maxRate);
        Task<InfluencerProfileDto> CreateInfluencerProfileAsync(Guid userId, CreateInfluencerProfileRequest request);
        Task<InfluencerProfileDto> UpdateInfluencerProfileAsync(Guid id, CreateInfluencerProfileRequest request);
        Task<bool> DeleteInfluencerProfileAsync(Guid id);
    }
}