using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Authentication and authorization endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IPermissionService _permissionService;
        private readonly IUserRepository _userRepository;

        public AuthController(
            IAuthService authService,
            INavigationService navigationService,
            IPermissionService permissionService,
            IUserRepository userRepository)
        {
            _authService = authService;
            _navigationService = navigationService;
            _permissionService = permissionService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Authenticate user and get JWT token
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns>Authentication response with JWT token</returns>
        /// <response code="200">Returns the authentication token and user information</response>
        /// <response code="400">Invalid credentials or request</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            
            if (!response.Success)
                return BadRequest(response);
            
            return Ok(response);
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="request">User registration information</param>
        /// <returns>Authentication response with JWT token</returns>
        /// <response code="200">Returns the authentication token and user information</response>
        /// <response code="400">Invalid registration data or user already exists</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            
            if (!response.Success)
                return BadRequest(response);
            
            return Ok(response);
        }

        /// <summary>
        /// Get current authenticated user information including permissions and navigation
        /// </summary>
        /// <returns>Current user details, roles, permissions, and navigation items</returns>
        /// <response code="200">Returns the current user information</response>
        /// <response code="401">Unauthorized - Invalid or missing token</response>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> GetCurrentUser()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            
            // Get user details
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            // Get permissions and navigation items
            var permissions = await _permissionService.GetUserPermissionsAsync(userId);
            var navigationItems = await _navigationService.GetUserNavigationAsync(userId);

            return Ok(new
            {
                UserId = userId,
                Email = email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles,
                Permissions = permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Description = p.Description,
                    Module = p.Module,
                    Action = p.Action
                }).ToList(),
                NavigationItems = MapNavigationItems(navigationItems)
            });
        }

        private List<NavigationItemDto> MapNavigationItems(IEnumerable<Core.Models.NavigationItem> items)
        {
            var dtos = new List<NavigationItemDto>();
            
            foreach (var item in items)
            {
                var dto = new NavigationItemDto
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
                    Children = item.Children != null ? MapNavigationItems(item.Children) : new List<NavigationItemDto>()
                };
                dtos.Add(dto);
            }
            
            return dtos;
        }
    }
}