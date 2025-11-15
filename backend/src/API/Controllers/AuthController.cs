using System.Threading.Tasks;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
        /// Get current authenticated user information
        /// </summary>
        /// <returns>Current user details and roles</returns>
        /// <response code="200">Returns the current user information</response>
        /// <response code="401">Unauthorized - Invalid or missing token</response>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public ActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            
            return Ok(new
            {
                UserId = userId,
                Email = email,
                Roles = roles
            });
        }
    }
}