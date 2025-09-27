using System;
using System.Threading.Tasks;
using InfluencerMarketplace.Shared.DTOs.Auth;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<string> GenerateJwtTokenAsync(Guid userId, string email, string[] roles);
    }
}