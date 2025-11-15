using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;
using InfluencerMarketplace.Shared.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly INavigationService _navigationService;
        private readonly IPermissionService _permissionService;

        public AuthService(
            IUserRepository userRepository, 
            IConfiguration configuration,
            INavigationService navigationService,
            IPermissionService permissionService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _navigationService = navigationService;
            _permissionService = permissionService;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var roles = await _userRepository.GetUserRolesAsync(user.Id);
            var token = await GenerateJwtTokenAsync(user.Id, user.Email, roles.ToArray());

            // Get user permissions and navigation items
            var permissions = await _permissionService.GetUserPermissionsAsync(user.Id);
            var navigationItems = await _navigationService.GetUserNavigationAsync(user.Id);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(7),
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList(),
                Permissions = permissions.Select(p => new Shared.DTOs.Auth.PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Description = p.Description,
                    Module = p.Module,
                    Action = p.Action
                }).ToList(),
                NavigationItems = MapNavigationItems(navigationItems),
                Message = "Login successful."
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email is already registered."
                };
            }

            var passwordHash = HashPassword(request.Password);
            
            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            var userId = await _userRepository.CreateAsync(user);
            
            // Assign role based on user type
            var roleName = request.UserType == "Brand" ? "Brand" : "Influencer";
            await _userRepository.AddUserToRoleAsync(userId, roleName);
            
            var roles = new[] { roleName };
            var token = await GenerateJwtTokenAsync(userId, user.Email, roles);

            // Get user permissions and navigation items
            var permissions = await _permissionService.GetUserPermissionsAsync(userId);
            var navigationItems = await _navigationService.GetUserNavigationAsync(userId);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(7),
                UserId = userId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList(),
                Permissions = permissions.Select(p => new Shared.DTOs.Auth.PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Description = p.Description,
                    Module = p.Module,
                    Action = p.Action
                }).ToList(),
                NavigationItems = MapNavigationItems(navigationItems),
                Message = "Registration successful."
            };
        }

        public async Task<string> GenerateJwtTokenAsync(Guid userId, string email, string[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(7);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            var result = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, result, salt.Length, hash.Length);
            
            return Convert.ToBase64String(result);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var bytes = Convert.FromBase64String(storedHash);
            var salt = new byte[128 / 8]; // HMACSHA512 key size
            Buffer.BlockCopy(bytes, 0, salt, 0, salt.Length);
            
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != bytes[salt.Length + i])
                    return false;
            }
            
            return true;
        }

        private List<Shared.DTOs.Auth.NavigationItemDto> MapNavigationItems(IEnumerable<Core.Models.NavigationItem> items)
        {
            var dtos = new List<Shared.DTOs.Auth.NavigationItemDto>();
            
            foreach (var item in items)
            {
                var dto = new Shared.DTOs.Auth.NavigationItemDto
                {
                    Id = item.Id,
                    Label = item.Label,
                    Icon = item.Icon,
                    Url = item.Url,
                    Order = item.Order,
                    ParentId = item.ParentId,
                    GroupId = item.GroupId,
                    GroupName = item.Group?.Name,
                    GroupOrder = item.Group?.Order,
                    Children = item.Children != null ? MapNavigationItems(item.Children) : new List<Shared.DTOs.Auth.NavigationItemDto>()
                };
                dtos.Add(dto);
            }
            
            return dtos;
        }
    }
}